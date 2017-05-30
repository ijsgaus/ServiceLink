using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceLink.Exceptions;
using ServiceLink.Monads;

namespace ServiceLink.Bus
{
    internal class Publisher<TSerializer, TDeserializer> : IPublisher, IWorker
        where TSerializer : IBusSerializer
        where TDeserializer : IBusDeserializer

    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _provider;
        private readonly IBusSerializer _serializer;
        private readonly IBusDeserializer _deserializer;
        private readonly IDisposable _stopper;

        public Publisher(ILogger<Publisher<TSerializer, TDeserializer>> logger, 
            IServiceProvider provider, IEnumerable<IPublishSafeStore> safePoints)
        {
            _logger = logger;
            _provider = provider;
            _serializer = provider.GetService<TSerializer>();
            _deserializer = provider.GetService<TDeserializer>();
            _stopper = _deserializer == null ? Disposable.Empty : RunRepublishing(safePoints);
        }


        public Task PublishAsync<T>(T message, CancellationToken? token = null)
        {
            _logger.LogTrace("Publishing message {@message}", message);
            var publisher = _provider.GetRequiredService<IPublisher<T>>();
            _logger.LogTrace($"Publishing with {publisher}");
            var task = publisher.PreparePublish(message)(token ?? CancellationToken.None);
            task.ContinueWith(_ => _logger.LogTrace("Published message {@message}", message),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(t => _logger.LogError(0, t.Exception, "On publishing {message}", message), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public Action PublishSafe<T>(T message, IPublishSafePoint safer)
        {
            if(_serializer == null)
                throw new ServiceLinkException("Safe publishing is not properly configured - no serializer provided");
            if (safer == null) throw new ArgumentNullException(nameof(safer));
            _logger.LogTrace("Safe publishing message {@message}", message);
            
            var serialized = _serializer.Serialize(message).Unwrap();
            var lease = safer.SavePublishing(serialized);
            return Helper.Prepare(_logger, _provider, lease, message);
        }

        

        private IDisposable RunRepublishing(IEnumerable<IPublishSafeStore> points)
        {
            var genericMethod = typeof(Helper).GetMethods().First(p => p.Name == nameof(Helper.Prepare));
            return new CompositeDisposable(
                points.Select(point =>
                    Observable
                        .Interval(point.CheckInterval)
                        .Subscribe(_ =>
                        {
                            while (true)
                            {
                                var (serialized, lease) = point.GetAwaited();
                                if (serialized == null) break;
                                try
                                {
                                    var (type, msg) = _deserializer.Deserialize(serialized).Unwrap();
                                    if(type == null)
                                        throw new ServiceLinkException($"Cannot deserialize message {serialized.Type} {serialized.ContentType}");
                                    var method = genericMethod.MakeGenericMethod(type);
                                    var run = (Action) method.Invoke(null, new[] {_logger, _provider, lease, msg});
                                    run();
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(0, ex, $"When republishing {point.GetType()}");
                                    lease.Dispose();
                                }
                            }
                        })));
        }

        public void Dispose()
        {
            _stopper.Dispose();
        }

        private class Helper
        {
            public static Action Prepare<T>(ILogger logger, IServiceProvider provider, ILease lease, T message)
            {
                var publisher = provider.GetRequiredService<IPublisher<T>>();

                logger.LogTrace($"Publisher used {publisher}");
                var publishing = publisher.PreparePublish(message);
                return () =>
                {
                    var tokenSource = new CancellationTokenSource();
                    var publish = publishing(tokenSource.Token).ToObservable();
                    var releaser = Observable.Interval(lease.LeaseInterval).Select(_ => Unit.Default);
                    releaser.TakeUntil(publish).Subscribe(_ =>
                    {
                        if (!lease.Renew())
                        {
                            tokenSource.Cancel();
                            tokenSource.Dispose();
                            lease.Dispose();
                        }
                    });
                    publish.Subscribe(_ =>
                    {
                        lease.Dispose();
                        logger.LogTrace("Published {message}", message);
                    }, ex => logger.LogError(0, ex, "Publishing {message}", message));
                };
            }
        }
    }
}