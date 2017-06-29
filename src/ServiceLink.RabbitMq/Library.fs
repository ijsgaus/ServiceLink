namespace ServiceLink.RabbitMq
open System
open ServiceLink
open RabbitLink
open RabbitLink.Consumer
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.Abstractions

module Consume =
    let receiveLoop (logger : ILogger) onNext consumer deserialize =
        async {
            logger.LogTrace("Receive loop started");
            while true do
                let! msg = Consumer.getMessage consumer
                logger.LogTrace("Message recieved {@message}", msg);
                let s = Message.toSerialized msg
                match deserialize s with
                | Ok r -> 
                    let ack = { Message = r; Confirm = Message.confirmByAck logger msg >> Async.Start } :> IAck<_>
                    onNext ack
                | Error ex -> 
                    msg.NackAsync() |> ignore
                    LoggerExtensions.LogWarning(logger, EventId(0) , ex :> Exception  , "On deserialize {@message} to type {type}", msg, typeof(TMessage))
        }
    
