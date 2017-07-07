namespace ServiceLink
open System
open System.Reflection


    

type LinkService = {
    ServiceId : ServiceId
    Provider : InfoProvider
}

type NotifyEndpoint = {
    ServiceId : LinkService
    EndpointId : EndpointId
    Provider : InfoProvider
    MessageType : Type
}

