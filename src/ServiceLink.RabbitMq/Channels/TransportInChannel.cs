using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Consumer;
using RabbitLink.Messaging;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Channels
{
    internal class TransportInChannel : ITransportInChannel<byte[]>
    {
        private readonly ILogger _logger;
        private readonly IObservable<IRecieveContext<byte[]>> _observable;

        public TransportInChannel(ILogger logger, Func<ILinkConsumer> consumerFactory)
        {
            _logger = logger;
            _observable = Observable.Create<IRecieveContext<byte[]>>(observer =>
            {
                var cancellation = new CancellationDisposable();
                var consumer = consumerFactory();
                async Task Loop()
                {
                    while (true)
                    {
                        var task = consumer.GetMessageAsync<byte[]>(cancellation.Token);
                        ILinkMessage<byte[]> msg;
                        try
                        {
                            msg = await task;
                        }
                        catch (Exception ex)
                        {
                            if (task.IsCanceled)
                                observer.OnCompleted();
                            else
                                observer.OnError(ex);
                            return;
                        }
                        observer.OnNext(new ReceiveContext(msg));
                    }
                }

                var tsk = Loop();
                return () =>
                {
                    cancellation.Dispose();
                    tsk.Wait();
                    consumer.Dispose();
                };
            });
        }
        
        public IDisposable Subscribe(IObserver<IRecieveContext<byte[]>> observer)
            => _observable.Subscribe(observer);

    }
}