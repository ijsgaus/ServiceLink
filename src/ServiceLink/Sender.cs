using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    internal class Sender<TSource, TService> : ISender<TSource, TService>
        where TService : class 
        where TSource : IMessageSource
    {
        private readonly TSource _source;
        private readonly IMetaProvider<TService> _metaProvider;
        private readonly ITransport _transport;

        public Sender(TSource source, IMetaProvider<TService> metaProvider, ITransport transport)
        {
            _source = source;
            _metaProvider = metaProvider;
            _transport = transport;
        }

        public Task Fire<TMessage>(Expression<Func<TService, Action<TMessage>>> selector, TMessage message, CancellationToken token)
        {
            var endPointInfo = new EndPointInfo(_metaProvider.ServiceName, _metaProvider.GetEndPointName(selector.GetMethod()));
            var producer = _transport.GetOrAddProducer<TMessage>(endPointInfo);
            return producer.Publish(message, _source, token);
        }

        public Guid Deliver<TMessage, TAnswer, TStore>(
            Expression<Func<TService, Func<TMessage, TAnswer>>> selector, TStore store, TMessage message, TimeSpan? resend)
            where TStore : IDeliveryStore
        {
            var endPointInfo = new EndPointInfo(_metaProvider.ServiceName,
                _metaProvider.GetEndPointName(selector.GetMethod()));
            var producer = _transport.GetOrAddProducer<TMessage>(endPointInfo);
            var lease = store.Save(endPointInfo, message);

            store.AfterCommit(() =>
            {
                var completeSource = new CancellationTokenSource();
                var leaseSource = new CancellationTokenSource();
                RenewLoop(lease, () => leaseSource.Cancel(), completeSource.Token)
                    .ContinueWith(_ => leaseSource.Dispose(), TaskContinuationOptions.ExecuteSynchronously);
                producer.Publish(message, _source, leaseSource.Token)
                    .ContinueWith(tsk =>
                    {
                        completeSource.Cancel();
                        completeSource.Dispose();
                        if (tsk.Status == TaskStatus.RanToCompletion)
                            lease.Renew(resend);
                    },TaskContinuationOptions.ExecuteSynchronously);
            });
            return lease.DeliveryId;
        }

        public void Publish<TMessage, TStore>(Expression<Func<TService, Action<TMessage>>> selector, TStore store,
            TMessage message) where TStore : IDeliveryStore
        {
            var endPointInfo = new EndPointInfo(_metaProvider.ServiceName,
                _metaProvider.GetEndPointName(selector.GetMethod()));
            var producer = _transport.GetOrAddProducer<TMessage>(endPointInfo);
            var lease = store.Save(endPointInfo, message);

            store.AfterCommit(() =>
            {
                var completeSource = new CancellationTokenSource();
                var leaseSource = new CancellationTokenSource();
                RenewLoop(lease, () => leaseSource.Cancel(), completeSource.Token)
                    .ContinueWith(_ => leaseSource.Dispose(), TaskContinuationOptions.ExecuteSynchronously);
                producer.Publish(message, _source, leaseSource.Token)
                    .ContinueWith(tsk =>
                    {
                        if (tsk.Status == TaskStatus.RanToCompletion)
                            lease.RemoveDelivery();
                        completeSource.Cancel();
                        completeSource.Dispose();
                    },TaskContinuationOptions.ExecuteSynchronously);
            });

        }

        private static async Task RenewLoop(IDeliveryLease lease, Action cancel, CancellationToken token)
        {
            do
            {
                await lease.WhenRenew(token);
                token.ThrowIfCancellationRequested();
                if (!lease.Renew())
                {
                    cancel();
                    break;
                }
                token.ThrowIfCancellationRequested();
            } while (true);
        }
    }
}