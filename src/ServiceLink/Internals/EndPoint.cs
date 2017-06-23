using System;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using ServiceLink.Transport;

namespace ServiceLink
{
    internal abstract class EndPointBase<TMessage, TAnswer>  
    {
        private readonly ILogger _logger;
        private readonly IEndPointEvents<TMessage> _eventer;
        private readonly IEndPointTransport<TMessage, TAnswer> _transport;
        private readonly ILinkStakeHolder _holder;

        internal EndPointBase(ILogger logger,
            [NotNull] EndPointInfo info, [NotNull] ILinkStakeHolder holder, [NotNull] IEndPointTransport<TMessage, TAnswer> transport,
            [NotNull] IEndPointEvents<TMessage> eventer)
        {
            _logger = logger;
            _eventer = eventer ?? throw new ArgumentNullException(nameof(eventer));
            _transport = transport ?? throw new ArgumentNullException(nameof(transport));
            _holder = holder ?? throw new ArgumentNullException(nameof(holder));
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Published = _eventer.Published.AsObservable();
        }

        public EndPointInfo Info { get; }
        public IObservable<TMessage> Published { get; }

        protected Task FireAsync(TMessage message, CancellationToken? token = null)
        {
            var parameters = new SendParams.WhenPublish(_holder.Name, null);
            var task = _transport.MessageProducer.Publish(message, parameters)(token ?? CancellationToken.None);
            task.ToObservable().Select(_ => message).Subscribe(_eventer.Published);
            return task;
        }

        protected Guid Publish(IDeliveryStore<TMessage> store, TMessage message, TimeSpan? retryInterval)
        {
            var lease = store.CreateDelivery(Info, message);
            var parameters = new SendParams.WhenPublish(_holder.Name, retryInterval.HasValue ? ((Guid?) lease.DeliveryId) : null);
            var produce = _transport.MessageProducer.Publish(message, parameters);
            store.AfterCommit(() =>
            {
                var completeSource = new CancellationTokenSource();
                var leaseSource = new CancellationTokenSource();
                RenewLoop(_logger, lease, () => leaseSource.Cancel(), completeSource.Token)
                    .ContinueWith(_ => leaseSource.Dispose(), TaskContinuationOptions.ExecuteSynchronously);
                var task = produce(leaseSource.Token);
                task.ContinueWith(tsk =>
                {
                    if (tsk.Status == TaskStatus.RanToCompletion)
                        if(retryInterval == null)
                            lease.RemoveDelivery();
                        else
                            lease.Renew(retryInterval.Value);
                    completeSource.Cancel();
                    completeSource.Dispose();
                },TaskContinuationOptions.ExecuteSynchronously);
                task.ToObservable().Select(_ => message).Subscribe(_eventer.Published);
            });
            return lease.DeliveryId;
        }

        protected IDisposable Subscibe(Func<Envelope, TMessage, Answer<TAnswer>> subscriber)
        {
            return _transport.MessageConsumer.Subscribe(
                AsConsumeSync(_logger, subscriber, _transport.AnswerProducer, h => new SendParams.WhenAnswer(_holder.Name, h.DeliveryId)), true);
        }
        
        protected IDisposable Subscibe(Func<Envelope, TMessage, CancellationToken, Task<Answer<TAnswer>>> subscriber)
        {
            return _transport.MessageConsumer.Subscribe(
                AsConsumeAsync(_logger, subscriber, _transport.AnswerProducer, h => new SendParams.WhenAnswer(_holder.Name, h.DeliveryId)), false);
        }

        private static Func<Envelope, TMessage, CancellationToken, Task<AnswerKind>> AsConsumeSync(ILogger logger,
            Func<Envelope, TMessage, Answer<TAnswer>> subscriber, IProducer<TAnswer> producer,
            Func<Envelope.Answer, SendParams> paramFactory)
            => (header, msg, token) => 
            {
                using (logger.BeginScope("Reciving message {@message}", msg))
                {
                    try
                    {
                        var result = subscriber(header, msg);
                        if (header is Envelope.Answer envAnswer)
                        
                            result.Match(ans =>
                                {
                                    logger.LogTrace("Sending answer");
                                    producer.Publish(ans, paramFactory(envAnswer))(CancellationToken.None);
                                },
                                () => { }, () => { });
                        
                        logger.LogTrace("Recieved {@result}", result);
                        return Task.FromResult(result.Kind);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(0, ex, "Error reciving");
                        throw;
                    }
                }
                    
                
            };

        
        private static Func<Envelope, TMessage, CancellationToken, Task<AnswerKind>> AsConsumeAsync(ILogger logger,
            Func<Envelope, TMessage, CancellationToken, Task<Answer<TAnswer>>> subscriber, IProducer<TAnswer> producer,
            Func<Envelope.Answer, SendParams> paramFactory)
            => async (header, msg, token) =>
            {
                using (logger.BeginScope("Reciving message {@message}", msg))
                {
                    try
                    {
                        var result = await subscriber(header, msg, token);
                        if (header is Envelope.Answer envAns)
                        
                            result.Match(ans =>
                                {
                                    logger.LogTrace("Sending answer");
                                    producer.Publish(ans, paramFactory(envAns))(token);
                                },
                                () => { }, () => { });
                        
                        logger.LogTrace("Recieved {@result}", result);
                        return result.Kind;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(0, ex, "Error reciving");
                        throw;
                    }
                }
            };


        private static async Task RenewLoop<TMessage>(ILogger logger,IDeliveryLease<TMessage> lease, Action cancel, CancellationToken token)
        {
            using (logger.BeginScope("Delivery publishing {@delivery}", lease.DeliveryId))
            {
                do
                {
                    logger.LogTrace("Awaiting renew");
                    await lease.WhenRenew(token);
                    token.ThrowIfCancellationRequested();
                    logger.LogTrace("Renewing");
                    if (!lease.Renew())
                    {
                        logger.LogTrace("Renew failed, cancelling");
                        cancel();
                        break;
                    }
                    token.ThrowIfCancellationRequested();
                } while (true);
            }
        }
    }

    internal class EndPoint<TService, TMessage> : EndPointBase<TMessage, ValueTuple>, IEndPoint<TMessage> 
        where TService : class
    {
        private readonly ILogger<EndPoint<TService, TMessage>> _logger;
        private readonly TimeSpan? _defaultRetry;

        public EndPoint(ILogger<EndPoint<TService, TMessage>> logger,
            [NotNull] EndPointInfo info, [NotNull] ILinkStakeHolder holder, [NotNull] IEndPointTransport<TMessage, ValueTuple> transport,
            [NotNull] IEndPointEvents<TMessage> eventer, TimeSpan? defaultRetry) : base(logger, info, holder, transport, eventer)
        {
            _logger = logger;
            _defaultRetry = defaultRetry;
        }

        Task IEndPoint<TMessage>.FireAsync(TMessage message, CancellationToken? token)
            => _logger.WithLog(() => FireAsync(message, token), "Fire {@message}", message);

        Guid IEndPoint<TMessage>.Publish(IDeliveryStore<TMessage> store, TMessage message, TimeSpan? retryInterval)
        {
            var retry = _defaultRetry == null ? null : retryInterval ?? _defaultRetry;
            return _logger.WithLog(() => Publish(store, message, retry),
                "Publishing with confirm {@message}, retry {@retry}", message, retry);
        }

        public IDisposable Subscibe(Func<Envelope, TMessage, Answer<ValueTuple>> subscriber)
            => _logger.WithLog(() => Subscibe(subscriber), "Subscription");
    }

    internal class EndPoint<TService, TMessage, TAnswer> : EndPointBase<TMessage, TAnswer>, IEndPoint<TMessage, TAnswer>
        where TService : class 
    {
        private readonly ILogger<EndPoint<TService, TMessage, TAnswer>> _logger;
        private readonly TimeSpan? _defaultRetry;

        public EndPoint(ILogger<EndPoint<TService, TMessage, TAnswer>> logger, [NotNull] EndPointInfo info,
            [NotNull] ILinkStakeHolder holder,
            [NotNull] IEndPointTransport<TMessage, TAnswer> transport, [NotNull] IEndPointEvents<TMessage> eventer, TimeSpan? defaultRetry) : base(
            logger, info, holder, transport, eventer)
        {
            _logger = logger;
            _defaultRetry = defaultRetry;
        }

        Guid IEndPoint<TMessage, TAnswer>.Publish(IDeliveryStore<TMessage> store, TMessage message, TimeSpan? retryInterval)
        {
            var retry = _defaultRetry == null ? null : retryInterval ?? _defaultRetry;
            return _logger.WithLog(() => Publish(store, message, retry),
                "Publishing with confirm {@message}, retry {@retry}", message, retry);
        }
    }
}