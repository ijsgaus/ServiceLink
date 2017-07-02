namespace ServiceLink
open System
open System.Runtime.CompilerServices

type EncodedType = EncodedType of string

type ContentType = ContentType of string

type Serialized<'t> = {
    ContentType : ContentType
    EncodedType : EncodedType
    Data : 't
}

type RawSerialized = Serialized<byte array>

type TextSerialized = Serialized<string>


type EndpointId = EndpointId of string
type EndpointKind = 
    | Notify
    | Cmd
    | Call
    


type IEndpoint = 
    abstract Id : EndpointId
    abstract Kind : EndpointKind
    abstract InType : Type
    abstract OutType : Type

type Notification<'message> = 
    | Notification of EndpointId
    interface IEndpoint with
        member __.Id =  let (Notification id) = __ in id
        member __.Kind = Notify
        member __.InType = typeof<'message>
        member __.OutType = typeof<unit>
        
type Command<'command> = 
    | Command of EndpointId
    interface IEndpoint with
        member __.Id =  let (Command id) = __ in id
        member __.Kind = Cmd
        member __.InType = typeof<'command>
        member __.OutType = typeof<unit>
        
        
type Callable<'args, 'result> = 
    | Callable of EndpointId
    interface IEndpoint with
        member __.Id =  let (Callable id) = __ in id
        member __.Kind = Call
        member __.InType = typeof<'args>
        member __.OutType = typeof<'result>


type ServiceDescription = IEndpoint list

type ServiceLink = ServiceLink

module ServiceLink =
    let 
    


