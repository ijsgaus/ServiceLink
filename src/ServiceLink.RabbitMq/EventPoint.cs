using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Consumer;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    internal class EventPoint<TMessage> : ITransportEventPoint<TMessage>
    {
        private readonly ISerializer<byte[]> _serializer;
        private readonly ILogger<EventPoint<TMessage>> _logger;
        private readonly Func<ISerialized<byte[]>, Func<CancellationToken, Task>> _publicator;
        private readonly Func<bool, ILinkConsumer> _consumerFactory;

        public EventPoint(ISerializer<byte[]> serializer, ILogger<EventPoint<TMessage>> logger,
            Func<ISerialized<byte[]>, Func<CancellationToken, Task>> publicator,
            Func<bool, ILinkConsumer> consumerFactory)
        {
            _serializer = serializer;
            _logger = logger;
            _publicator = publicator;
            _consumerFactory = consumerFactory;
        }

        private IDisposable CreateLogScope(TMessage message)
        {
            throw new NotImplementedException();
        }

        private Action<Task> OnPublishFinished(TMessage message)
        {
            throw new NotImplementedException();
        }

        public Func<CancellationToken, Task> PrepareSend(TMessage message)
        {
            var serialized = _serializer.Serialize(message);
            var publish = _publicator(serialized);
            return ct =>
            {
                var task = publish(ct);
                task.ContinueWith(OnPublishFinished(message), CancellationToken.None);
                return task;
            };
        }

        public IObservable<IAck<TMessage>> Connect(bool separate)
        {
            return Observable.Create<IAck<TMessage>>(p => ObservableFactory(p, separate));
        }
        
        private IDisposable ObservableFactory(IObserver<IAck<TMessage>> observer, bool separate)
        {
            var cancellation = new CancellationDisposable();
            var consumer = _consumerFactory(separate);
            async Task Loop()
            {
                while (!cancellation.Token.IsCancellationRequested)
                {
                    var msg = await consumer.GetMessageAsync<byte[]>(cancellation.Token);
                    var serialized = new Serialized<byte[]>(ContentType.Parse(msg.Properties.ContentType), 
                        EncodedType.Parse(msg.Properties.Type), msg.Body);
                    _serializer.TryDeserialize<TMessage>(serialized)
                        .Match(p =>  observer.OnNext(new Ack<TMessage>(p, ack =>
                        {
                            switch (ack)
                            {
                                case Ack.Ack:
                                    msg.AckAsync();
                                    break;
                                case Ack.Nack:
                                    msg.NackAsync();
                                    break;
                                case Ack.Requeue:
                                    msg.RequeueAsync();
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(ack), ack, null);
                            }
                        })), _ => msg.NackAsync());
                }
            }

            var task = Loop();
            
            Action Dispose(IDisposable cancel, Task tsk, IDisposable cnsmr)
                => () => {
                    cancel.Dispose();
                    tsk.ContinueWith(_ => { }).Wait();
                    cnsmr.Dispose();
                };

            return Disposable.Create(Dispose(cancellation, task, consumer));
        }
    }
}
