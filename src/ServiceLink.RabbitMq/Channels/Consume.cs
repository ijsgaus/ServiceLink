using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Consumer;
using RabbitLink.Messaging;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Channels
{
    internal static class Consume
    {
        public static Func<bool, IObservable<IAck<TMessage>>> MakeConnect<TMessage>(ILogger logger,
            ISerializer<byte[]> serializer,
            Func<bool, ILinkConsumer> consumerFactory)
            => separate =>
            {
                return Observable.Create(ToObservable<TMessage>(logger, p => consumerFactory(separate),
                    serializer));
            };
        
        
        
        
        private static Func<Task> ReceiveLoop<TMessage>(ILogger logger, IObserver<IAck<TMessage>> observer, 
            ILinkConsumer consumer, CancellationToken cancellation, ISerializer<byte[]> serializer)
        => async () =>
        {
            logger.LogTrace("Receive loop started");
            while (!cancellation.IsCancellationRequested)
            {
                var msg = await consumer.GetMessageAsync<byte[]>(cancellation);
                logger.LogTrace("Message recieved {@message}", msg);
                var serialized = SerializedFromMessage(msg);
                serializer.TryDeserialize<TMessage>(serialized)
                    .Match(p =>  observer.OnNext(CreateAck(p, ConfirmByAck(logger, msg))), ex =>
                    {
                        msg.NackAsync();
                        logger.LogWarning(0, ex, "On deserialize {@message} to type {type}", msg, typeof(TMessage));
                    });
            }
        }

        private static Serialized<byte[]> SerializedFromMessage(ILinkMessage<byte[]> msg)
        {
            return new Serialized<byte[]>(ContentType.Parse(msg.Properties.ContentType), 
                EncodedType.Parse(msg.Properties.Type), msg.Body);
        }

        private static Ack<TMessage> CreateAck<TMessage>(TMessage message, Action<AckKind> confirm)
        {
            return new Ack<TMessage>(message, confirm);
        }

        private static Action<AckKind> ConfirmByAck(ILogger logger, ILinkMessage<byte[]> msg)
        {
            return ack =>
            {
                logger.LogTrace("Message {ack} {@message}", ack, msg);
                switch (ack)
                {
                    case AckKind.Ack:
                        msg.AckAsync();
                        break;
                    case AckKind.Nack:
                        msg.NackAsync();
                        break;
                    case AckKind.Requeue:
                        msg.RequeueAsync();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(ack), ack, null);
                }
            };
        }

        private static Action StopLoop(IDisposable cancel, Task tsk, IDisposable cnsmr, ILogger logger)
            => () => 
            {
                cancel.Dispose();
                tsk.ContinueWith(_ => { }).Wait();
                cnsmr.Dispose();
                logger.LogTrace("Recieve loop end");
            };

        private static Func<IObserver<IAck<TMessage>>, IDisposable> ToObservable<TMessage>(
             ILogger logger, Func<ILinkConsumer> consumerFactory, ISerializer<byte[]> serializer)
            => observer =>
            {
                var cancellation = new CancellationDisposable();
                var consumer = consumerFactory();
                var task = ReceiveLoop(logger, observer, consumer, cancellation.Token, serializer)();
                return Disposable.Create(StopLoop(cancellation, task, consumer, logger));
            };

        
    }
    
}