namespace ServiceLink
open System
open System.Runtime.CompilerServices
open System.Reflection
open System.Linq.Expressions
open FSharp.Quotations
open FSharp.Quotations.Patterns

type EncodedType = EncodedType of string

type ContentType = ContentType of string

type Serialized<'t> = {
    ContentType : ContentType
    EncodedType : EncodedType
    Data : 't
}

type RawSerialized = Serialized<byte array>

type TextSerialized = Serialized<string>


type ServiceId = ServiceId of string

type HolderId = HolderId of string

type IHolder =
    abstract Id : HolderId

type EndpointId = {
    Service : ServiceId
    Name : string
}

    
type IEndpointMember = 
    abstract Id : EndpointId
    abstract Kind : EndpointKind
    abstract Holder : HolderId
    abstract InType : Type
    abstract OutType : Type
    abstract Member : MemberInfo

type IEndpointMember<'msg, 'ret> = 
    inherit IEndpointMember


type Ack<'t> =
    | Ack of 't
    | Nack
    | Requeue
    
    
type Consumer<'msg, 't> = 'msg -> Async<Ack<'t>>
type Executor = (unit -> Async<unit>) -> Async<unit>


type INotifyTransport<'message> =
    abstract sender : 'message -> Async<unit>  
    abstract recieve : Executor -> Consumer<'message, unit> -> IDisposable     

type INotifyEndpointMember<'message> = 
    inherit IEndpointMember<'message, unit>
    
type ICommandEndpointMember<'command> = 
    inherit IEndpointMember<'command, unit>
        
type ICallEndpointMember<'args, 'result> =
    inherit IEndpointMember<'args, 'result>


type ISerializer<'t> =
        abstract Serialize : 'msg -> Serialized<'t>
        abstract Deserialize : Serialized<'t> -> 'msg
    



type ServiceLinkConfiguration = {
    EndpointName : MemberInfo -> EndpointKind -> EndpointId
    RawSerializer : ISerializer<byte array>
}

module ServiceLinkConfiguration =
    let endpoint (memb : MemberInfo) kind config =
        config.EndpointName memb kind


type ServiceLink<'service> = 
    {
        Configuration : ServiceLinkConfiguration
    }    
    static member endpointId memb kind sl =
        sl.Configuration |> ServiceLinkConfiguration.endpoint memb kind  
    
type Notification<'service, 'message> = 
    { 
        Member : MemberInfo
        Service : ServiceLink<'service> 
        Holder : IHolder
    }
    interface INotifyEndpointMember<'message> with
        member __.Id =  let { Notification.Member = memb; Service = srv } = __ in ServiceLink<_>.endpointId memb Notify srv 
        member __.Kind = Notify
        member __.InType = typeof<'message>
        member __.OutType = typeof<unit>
        member __.Member = let { Notification.Member = memb } = __ in memb
        member __.Holder = __.Holder.Id
        
type Command<'service, 'command> = 
    {
        Member : MemberInfo
        Service : ServiceLink<'service>
        Holder : IHolder
    }
    interface ICommandEndpointMember<'command> with
        member __.Id =  let { Command.Member = memb; Service = srv } = __ in ServiceLink<_>.endpointId memb Cmd srv
        member __.Kind = Cmd
        member __.InType = typeof<'command>
        member __.OutType = typeof<unit>
        member __.Member = let { Command.Member = memb } = __ in memb
        member __.Holder = __.Holder.Id
        
type Callable<'service, 'args, 'result> = 
    {
        Member : MemberInfo
        Service : ServiceLink<'service>
        Holder : IHolder
    }
    interface ICallEndpointMember<'args, 'result> with
        member __.Id =  let { Callable.Member = memb; Service = srv } = __ in ServiceLink<_>.endpointId memb Call srv
        member __.Kind = Call
        member __.InType = typeof<'args>
        member __.OutType = typeof<'result>
        member __.Member = let { Callable.Member = memb } = __ in memb
        member __.Holder = __.Holder.Id

type ITransport = 
    abstract get : Notification<'service, 'message> -> INotifyTransport<'message>


[<AutoOpen>]
module Expressions =
    type LambdaExpression with
        member __.property () =
            match __.Body with
            | :? MemberExpression as me ->
                match me.Member with
                | :? PropertyInfo as pi -> Some pi
                | _ -> None
            | _ -> None

[<AutoOpen>]
module Expr =
    type Expr with
        member __.property () =
            match __ with
            | Lambda (_, PropertyGet(_, pi, _)) -> Some pi
            | _ -> None
                
module Result =
    let unwrap r =
        match r with
        | Ok i -> i
        | Error ex -> raise ex
    let fromOption err o =
        match o with
        | Some v -> Ok v
        | _ -> Error err

module ServiceLink =
    let create<'service> config : ServiceLink<'service> =
        { Configuration = config } 
    let notify (selector : Expr<'service -> Notify<'message>>) holder (sl : ServiceLink<'service>) : Result<Notification<'service, 'message>, Exception> =
        selector.property() 
            |> Option.map (fun p -> { Notification.Member = p; Service = sl; Holder = holder }) 
            |> Result.fromOption (ArgumentException "Invalid selector" :> Exception)
        
    let notifyEx (selector : Expression<Func<'service, Notify<'message>>>) holder (sl : ServiceLink<'service>) : Result<Notification<'service, 'message>, Exception> =
        selector.property() 
            |> Option.map (fun p -> { Notification.Member = p; Service = sl; Holder = holder }) 
            |> Result.fromOption (ArgumentException "Invalid selector" :> Exception)
            
    let command (selector : Expr<Func<'service, Command<'command>>>) holder (sl : ServiceLink<'service>) : Result<Command<'service, 'message>, Exception> =
        selector.property() 
            |> Option.map (fun p -> { Command.Member = p; Service = sl; Holder = holder }) 
            |> Result.fromOption (ArgumentException "Invalid selector" :> Exception)
            
    let commandEx (selector : Expression<Func<'service, Command<'command>>>) holder (sl : ServiceLink<'service>) : Result<Command<'service, 'message>, Exception> =
        selector.property() 
            |> Option.map (fun p -> { Command.Member = p; Service = sl; Holder = holder }) 
            |> Result.fromOption (ArgumentException "Invalid selector" :> Exception)
            
    let callable (selector : Expr<Func<'service, Call<'args, 'result>>>) holder (sl : ServiceLink<'service>) : Result<Callable<'service, 'args, 'result>, Exception> =
        selector.property() 
            |> Option.map (fun p -> { Callable.Member = p; Service = sl; Holder = holder }) 
            |> Result.fromOption (ArgumentException "Invalid selector" :> Exception)
            
    let callableEx (selector : Expression<Func<'service, Call<'args, 'result>>>) holder (sl : ServiceLink<'service>) : Result<Callable<'service, 'args, 'result>, Exception> =
        selector.property() 
            |> Option.map (fun p -> { Callable.Member = p; Service = sl; Holder = holder }) 
            |> Result.fromOption (ArgumentException "Invalid selector" :> Exception)

type ServiceLink =
    static member endpoint(ep, h, sl) = 
        ServiceLink.notify ep h sl |> Result.unwrap
    static member endpoint(ep, h, sl) = 
        ServiceLink.command ep h sl |> Result.unwrap
    static member endpoint(ep, h, sl) = 
        ServiceLink.callable ep h sl |> Result.unwrap

type ServiceLink<'service> with
    member __.Endpoint (ep, h) =
        ServiceLink.notifyEx ep h __  |> Result.unwrap 
    member __.Endpoint (ep, h) =
        ServiceLink.commandEx ep h __ |> Result.unwrap
    member __.Endpoint (ep, h) =
        ServiceLink.callableEx ep h __ |> Result.unwrap