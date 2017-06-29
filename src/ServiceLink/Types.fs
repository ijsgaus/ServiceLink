namespace ServiceLink
open System
open System.Runtime.CompilerServices

type AckKind =
    | Ack = 0
    | Nack = 1
    | Requeue = 2   

type EncodedType = EncodedType of string

type ContentType = ContentType of string

type Serialized<'t> = {
    ContentType : ContentType
    EncodedType : EncodedType
    Data : 't
}

type RawSerialized = Serialized<byte array>

type IAck<'t> =
    abstract Message : 't
    abstract Confirm : Action<AckKind>

type Ack<'t> = 
    {
        Message : 't
        Confirm : AckKind -> unit
    }
    interface IAck<'t> with
        member __.Message = __.Message
        member __.Confirm = Action<_>(__.Confirm)


