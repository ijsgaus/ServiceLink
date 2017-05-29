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
            IServiceProvider provider, IEnumerable<IPublishSafePoint> safePoints)
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
            var task = publisher.Publish(message, token);
            task.ContinueWith(_ => _logger.LogTrace("Published message {@message}", message),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(t => _logger.LogError(0, t.Exception, "On publishing {message}", message), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public void PublishSafe<T>(T message, IPublishSafePoint safer)
        {
            if(_serializer == null)
                throw new ServiceLinkException("Safe publishing is not properly configured - no serializer provided");
            if (safer == null) throw new ArgumentNullException(nameof(safer));
            _logger.LogTrace("Safe publishing message {@message}", message);
            
            var serialized = _serializer.Serialize(message);
            if(serialized == null)
                throw new ServiceLinkException($"Cannot serialize {message} with type {message?.GetType() ?? typeof(T)}");
            var tag = safer.SavePublishing(serialized);
            Helper.Publish(_logger, _provider, safer, message, tag);
        }

        

        private IDisposable RunRepublishing(IEnumerable<IPublishSafePoint> points)
        {
            var genericMethod = typeof(Helper).GetMethods().First(p => p.Name == nameof(Helper.Publish));
            return new CompositeDisposable(
                points.Select(point =>
                    Observable
                        .Interval(point.LeaseInterval)
                        .Subscribe(_ =>
                        {
                            while (true)
                            {
                                var (serialized, tag) = point.GetAwaited();
                                if (serialized == null) break;
                                try
                                {
                                    var (type, msg) = _deserializer.Deserialize(serialized);
                                    if(type == null)
                                        throw new ServiceLinkException($"Cannot deserialize message {serialized.Type} {serialized.ContentType} {tag}");
                                    var method = genericMethod.MakeGenericMethod(type);
                                    method.Invoke(null, new[] {_logger, _provider, point, msg, tag});

                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(0, ex, $"When republishing {point.GetType()}");
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
            public static void Publish<T>(ILogger logger, IServiceProvider provider, IPublishSafePoint safer, T message, object tag)
            {
                var publisher = provider.GetRequiredService<IPublisher<T>>();

                logger.LogTrace($"Publisher used {publisher}");
                var publish = publisher.Publish(message, CancellationToken.None).ToObservable();
                var releaser = Observable.Interval(safer.LeaseInterval).Select(_ => Unit.Default);
                releaser.TakeUntil(publish).Subscribe(_ => safer.ProlongateLease(tag));
                publish.Subscribe(_ =>
                {
                    safer.Complete(tag);
                    logger.LogTrace("Published {message}", message);
                });
            }
        }
    }
}