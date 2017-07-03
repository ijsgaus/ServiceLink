namespace ServiceLink.RabbitMq
open System
open System.Threading
open System.Threading.Tasks
open System.Collections.Concurrent
open RabbitLink
open RabbitLink.Messaging
open RabbitLink.Producer
open RabbitLink.Configuration
open RabbitLink.Topology
open ServiceLink

type ProduceParams = {
    MessageProps : LinkMessageProperties
    PublishProps : LinkPublishProperties
}

type IProducerConfig =
     abstract ExchangeName : string
     abstract ConfirmMode : bool
     abstract ExchangeFactory : Func<ILinkTopologyConfig , Task<ILinkExchage>>
     abstract ConfigBuilder : Action<ILinkProducerConfigurationBuilder>
      

type ProducerConfig<'msg> = 
    {
        ExchangeName : string
        ConfirmMode : bool
        ExchangeFactory : Func<ILinkTopologyConfig , Task<ILinkExchage>>
        ConfigBuilder : Action<ILinkProducerConfigurationBuilder>
        PropFactory : 'msg -> ProduceParams
        RawSerializer : 'msg -> RawSerialized
    }
    interface IProducerConfig with
        member __.ExchangeName = __.ExchangeName
        member __.ConfirmMode = __.ConfirmMode
        member __.ExchangeFactory = __.ExchangeFactory
        member __.ConfigBuilder = __.ConfigBuilder

type ILinkConfig = 
         abstract ConnectionTimeout : TimeSpan
         abstract RecoveryInterval : Option<TimeSpan>
         abstract PublishTimeout : TimeSpan
         abstract NotifyProducer: IEndpointMember<'msg, unit> -> ProducerConfig<'msg>
    
  
type LinkOwner = private {
    Config : ILinkConfig
    Link : Lazy<Link> 
    Producers : ConcurrentDictionary<string * bool, ILinkProducer>
}

module RabbitLink =
    let defaultProducer ()  =
        let exchcfg e p  =
            let t = fun (f: ILinkTopologyConfig) -> f.ExchangeDeclare(e, LinkExchangeType.Direct)
            Func<_, _>(t)
        let prodcfg e p = 
            Action<_>( fun (f: ILinkProducerConfigurationBuilder) -> f.ConfirmsMode(p) |> ignore)
        ( exchcfg, prodcfg )
            
    
    let create url config (slcfg : ServiceLinkConfiguration) =
        let makeLink (cfg : ILinkConfig) =
            let build (bld : ILinkConfigurationBuilder) =
                bld.ConnectionTimeout(cfg.ConnectionTimeout) |> ignore
                cfg.RecoveryInterval |> Option.map (fun t -> bld.ChannelRecoveryInterval(t)) |> ignore
                bld.ProducerPublishTimeout(cfg.PublishTimeout) |> ignore
            new Link(url, Action<_>(build))
        let cfg = {
            new ILinkConfig with
                member __.ConnectionTimeout = TimeSpan.FromSeconds(10.0)
                member __.RecoveryInterval = None
                member __.PublishTimeout = Timeout.InfiniteTimeSpan
                member __.NotifyProducer ep = 
                        let (ServiceId en) = ep.Id.Service 
                        let (HolderId holderName) = ep.Holder 
                        let (exp, pp) = defaultProducer ()
                        {
                            ExchangeName =  en
                            ConfirmMode = true
                            ExchangeFactory = exp en true
                            ConfigBuilder = pp en true
                            PropFactory = fun msg ->
                                {
                                    MessageProps = LinkMessageProperties(AppId = holderName)
                                    PublishProps = LinkPublishProperties(RoutingKey = ep.Id.Name)
                                }
                            RawSerializer = fun m -> slcfg.RawSerializer.Serialize m
                        }
                        
        }
        let cfg = config cfg
        {
             Link = Lazy(Func<_>(fun () -> makeLink cfg))
             Producers = ConcurrentDictionary<_, _>()
             Config = cfg
        }
        
    let getOrAddProducer (prod : IProducerConfig)  o =
        o.Producers.GetOrAdd((prod.ExchangeName, prod.ConfirmMode) , fun _ ->  o.Link.Value.CreateProducer(prod.ExchangeFactory, null, prod.ConfigBuilder))
    
    let applySerialization prop sr =
        let (ContentType contentType) = sr.ContentType
        let (EncodedType et) = sr.EncodedType
        prop.MessageProps.ContentType <- contentType
        prop.MessageProps.Type <- et
        prop
        
        
        
        
type NotifyTransport<'service, 'message> = {
    LinkOwner : LinkOwner
    Notification : Notification<'service, 'message>
}

module NotifyTransport =
    let create owner notify =
        { LinkOwner = owner; Notification = notify }
    let prepare notify msg =
        let {LinkOwner = owner; Notification = note } = notify
        let producerConfig = owner.Config.NotifyProducer note
        let producer = RabbitLink.getOrAddProducer producerConfig owner
        let smsg = producerConfig.RawSerializer msg
        let cfg = producerConfig.PropFactory msg
        let cfg = RabbitLink.applySerialization cfg smsg
        fun ct -> producer.PublishAsync(smsg.Data, cfg.MessageProps, cfg.PublishProps, ct)
        