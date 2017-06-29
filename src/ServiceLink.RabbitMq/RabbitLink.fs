namespace ServiceLink.RabbitMq
open System
open ServiceLink
open RabbitLink.Messaging
open RabbitLink.Consumer
open Microsoft.Extensions.Logging

module RabbitLink =
    module Message =
        let toSerialized (msg : ILinkMessage<byte[]>) : RawSerialized = 
            { 
                ContentType = msg.Properties.ContentType |> ContentType 
                EncodedType = msg.Properties.Type |> EncodedType         
                Data = msg.Body
            }
        let confirmByAck (logger : ILogger) (msg : ILinkMessage<_>) ack =
            logger.LogTrace("Message {ack} {@message}", ack, msg)
            match ack with
            | AckKind.Ack -> msg.AckAsync() |> Async.AwaitTask
            | AckKind.Nack -> msg.NackAsync() |> Async.AwaitTask
            | AckKind.Requeue -> msg.RequeueAsync() |> Async.AwaitTask
            | _ -> ArgumentOutOfRangeException("ack", ack, null) |> raise;
    module Consumer =
        let getMessage (consumer : ILinkConsumer) =
            async {
                let task = consumer.GetMessageAsync<_>(Nullable(Async.DefaultCancellationToken))
                return! task |> Async.AwaitTask
            }
            