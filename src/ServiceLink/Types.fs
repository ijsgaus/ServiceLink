namespace ServiceLink
open System
open System.Runtime.CompilerServices
open System.Reflection

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

    


type IEndpointMember = 
    abstract Id : EndpointId
    abstract Kind : EndpointKind
    abstract InType : Type
    abstract OutType : Type
    abstract Member : MemberInfo
    

    
    
    
type Notification<'service, 'message> = 
    | Notification of MemberInfo
    interface IEndpointMember with
        member __.Id =  let (Notification memb) = __ in EndpointId memb.Name
        member __.Kind = Notify
        member __.InType = typeof<'message>
        member __.OutType = typeof<unit>
        member __.Member = let (Notification mem) = __ in mem
        
type Command<'service, 'command> = 
    | Command of MemberInfo
    interface IEndpointMember with
        member __.Id =  let (Command memb) = __ in EndpointId memb.Name
        member __.Kind = Cmd
        member __.InType = typeof<'command>
        member __.OutType = typeof<unit>
        member __.Member = let (Command mem) = __ in mem
        
        
type Callable<'service, 'args, 'result> = 
    | Callable of MemberInfo
    interface IEndpointMember with
        member __.Id =  let (Callable memb) = __ in EndpointId memb.Name
        member __.Kind = Call
        member __.InType = typeof<'args>
        member __.OutType = typeof<'result>
        member __.Member = let (Callable mem) = __ in mem




type ITransport = interface end


type ServiceDescription = IEndpointMember list

type ServiceLinkConfiguration = ServiceLinkConfiguration

type ServiceLink = ServiceLink

(*module ServiceLink =
    let*) 
    


