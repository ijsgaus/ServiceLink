using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Consumer;
using RabbitLink.Messaging;
using ServiceLink.Serializers;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Channels
{
    internal static class Consume
    {
        public static IObservable<IAck<TMessage>> MakeConnect<TMessage>(ILogger logger,
            ISerializer<byte[]> serializer,
            Func<ILinkConsumer> consumerFactory, AckConfirmer confirmer)
        {
            return Observable.Create(ToObservable<TMessage>(logger, consumerFactory,
                serializer, confirmer));
        }

        public delegate Action<ILogger, AckKind> AckConfirmer(ILinkMessage<byte[]> message);


        private static Func<Task> ReceiveLoop<TMessage>(ILogger logger, Action<IAck<TMessage>> onNext,
            ILinkConsumer consumer, CancellationToken cancellation, ISerializer<byte[]> serializer,
            AckConfirmer confirm)
            => async () =>
            {
                logger.LogTrace("Receive loop started");
                while (!cancellation.IsCancellationRequested)
                {
                    var msg = await consumer.GetMessageAsync<byte[]>(cancellation);
                    logger.With("@message", msg).LogTrace("Message recieved");
                    var serialized = SerializedFromMessage(msg);
                    serializer.TryDeserialize<TMessage>(serialized)
                        .Match(p => onNext(CreateAck(p, ack => confirm(msg)(logger, ack))), ex =>
                        {
                            msg.NackAsync();
                            logger.LogWarning(0, ex, "On deserialize {@message} to type {type}", msg, typeof(TMessage));
                        });
                }
            };

        private static Serialized<byte[]> SerializedFromMessage(ILinkMessage<byte[]> msg)
        {
            return new Serialized<byte[]>(ContentType.Parse(msg.Properties.ContentType), 
                EncodedType.Parse(msg.Properties.Type), msg.Body);
        }

        private static Ack<TMessage> CreateAck<TMessage>(TMessage message, Action<AckKind> confirm)
        {
            return new Ack<TMessage>(message, confirm);
        }


        public static Action<ILogger, AckKind> ConfirmByAck(ILinkMessage<byte[]> msg)
        {
            return (logger, ack) =>
            {
                
                logger.With(("ack", ack), ("@message", msg)).LogTrace("Message acknowledged");
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
             ILogger logger, Func<ILinkConsumer> consumerFactory, ISerializer<byte[]> serializer, AckConfirmer confirmer)
            => observer =>
            {
                var cancellation = new CancellationDisposable();
                var consumer = consumerFactory();
                var task = ReceiveLoop<TMessage>(logger, p => observer.OnNext(p), consumer, cancellation.Token, serializer, confirmer)();
                return Disposable.Create(StopLoop(cancellation, task, consumer, logger));
            };

        
    }
    
}