namespace ServiceLink
open System
open System.Runtime.CompilerServices

    

type LinkResult<'t> = Result<'t option, Exception>

module OptResult =
    let bindOpt f r =
        match r with
        | Ok (None) -> f None
        | Ok (Some v) ->
            let r1 = f (Some v)
            match r1 with
            | Ok (None) -> r
            | _ -> r1
        | Error e -> Error e 
    let map f r =
        r |> Result.map (Option.map f)
    let bind f r =
        match r with
        | Ok (None) -> Ok(None) 
        | Ok (Some v) -> f v
        | Error e -> Error e
    
        

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Configuration = 
    type ServiceId = ServiceId of string
    type EventId<'t> = EventId of string
    type TransportFactory = TransportFactory
    
    type IConfigProvider = 

        abstract GetServiceId<'t> : ServiceId option -> LinkResult<ServiceId> 
        abstract GetEventId<'t> : ServiceId -> EventId<'t> -> LinkResult

    module ConfigProvider =
        let private apply v f providers =
            providers 
                |> Seq.map f
                |> Seq.fold (fun st p -> st |> OptResult.bindOpt p) v
        let combine (providers: IConfigProvider seq) =
            { 
                new IConfigProvider with
                    member __.GetServiceId<'t> v = 
                        providers |> apply (Ok v) (fun p -> p.GetServiceId<'t>) 
            }

        

        
    type ManagerConfig = {
        Transport : TransportFactory
        Provider : IConfigProvider
    }

    

    
    
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ServiceLink = 
    open Configuration
    type Service<'t> = {
        ServiceId : ServiceId
        Config : ManagerConfig
    }

    type EventEndPoint<'t> = {
        ServiceId : ServiceId
        EventId : EventId
        Config : ManagerConfig
    }

    let createService<'t> cfg id : Service<'t> =  {
        ServiceId = id
        Config = cfg
    }
    let getService<'t> cfg = 
        cfg.Provider.GetServiceId<'t> None |> OptResult.map (createService<'t> cfg)
