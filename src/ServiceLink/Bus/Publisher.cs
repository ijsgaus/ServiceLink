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

namespace ServiceLink.Bus
{
    internal class Publisher : IPublisher, IWorker
    {
        private readonly ILogger<Publisher> _logger;
        private readonly IServiceProvider _provider;
        private readonly ISafePublishSerializer _serializer;
        private readonly ISafePublishDeserializer _deserializer;
        private readonly IDisposable _stopper;

        public Publisher(ILogger<Publisher> logger, IServiceProvider provider, ISafePublishSerializer serializer,
            ISafePublishDeserializer deserializer,
            IEnumerable<IPublishSafePoint> safePoints)
        {
            _logger = logger;
            _provider = provider;
            _serializer = serializer;
            _deserializer = deserializer;
            _stopper = RunRepublishing(safePoints);
        }


        public Task PublishAsync<T>(T message, CancellationToken? token = null)
        {
            _logger.LogTrace("Publishing message {@message}", message);
            var publisher = _provider.GetRequiredService<IPublisher<T>>();
            _logger.LogTrace($"Publisher used {publisher}");
            var task = publisher.Publish(message, token);
            task.ContinueWith(_ => _logger.LogTrace("Published message {@message}", message),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(t => _logger.LogTrace(0, t.Exception, "On publishing {message}", message), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public void PublishSafe<T>(T message, IPublishSafePoint safer)
        {
            if (safer == null) throw new ArgumentNullException(nameof(safer));
            _logger.LogTrace("Safe publishing message {@message}", message);
            
            var serialized = _serializer.Serialize(message);
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