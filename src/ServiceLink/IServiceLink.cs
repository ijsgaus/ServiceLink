using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{

    public interface ISender<TSource, TService>
        where TSource : IMessageSource
    {
        Task Fire<TMessage>(Expression<Func<TService, IEndPoint<TMessage>>> selector, TMessage message, CancellationToken token);

        //Task Fire<TMessage>(Expression<Func<TService, Action<TMessage>>> selector, TMessage message, CancellationToken token);
        
        void Publish<TMessage, TStore>(Expression<Func<TService, IEndPoint<TMessage>>> selector, TStore store,
            TMessage message)
            where TStore : IDeliveryStore;

        Guid Deliver<TMessage, TAnswer, TStore>(Expression<Func<TService, IEndPoint<TMessage, TAnswer>>> selector, TStore store, TMessage message, TimeSpan? resend)
            where TStore : IDeliveryStore;
    }

    public interface IDeliveryStore
    {
        IDeliveryLease Save<TMessage>(EndPointInfo info, TMessage message);
        void AfterCommit(Action action);
    }

    public interface IDeliveryLease 
    {
        Guid DeliveryId { get; }
        bool Renew(TimeSpan? interval = null);
        Task WhenRenew(CancellationToken token);
        void RemoveDelivery();
    }

    public interface IMessageSource
    {
        string Application { get; }
    }


    public interface ITransport
    {
        IProducer<TMessage> GetOrAddProducer<TMessage>(EndPointInfo info);
    }

    public class EndPointInfo
    {
        public EndPointInfo(string serviceName, string endpointName)
        {
            ServiceName = serviceName;
            EndpointName = endpointName;
        }

        public string ServiceName { get; }
        public string EndpointName { get; }
    }
    

    public interface IProducer<in TMessage>
    {
        Task Publish(TMessage message, IMessageSource source, CancellationToken token);
    }

    public interface IMetaProvider<TService>
        where TService : class
    {
        string ServiceName { get; }
        string GetEndPointName(PropertyInfo endPoint);
    }

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

        public Task Fire<TMessage>(Expression<Func<TService, IEndPoint<TMessage>>> selector, TMessage message, CancellationToken token)
        {
            var endPointInfo = new EndPointInfo(_metaProvider.ServiceName, _metaProvider.GetEndPointName(selector.GetProperty()));
            var producer = _transport.GetOrAddProducer<TMessage>(endPointInfo);
            return producer.Publish(message, _source, token);
        }

        public Guid Deliver<TMessage, TAnswer, TStore>(
            Expression<Func<TService, IEndPoint<TMessage, TAnswer>>> selector, TStore store, TMessage message, TimeSpan? resend)
            where TStore : IDeliveryStore
        {
            var endPointInfo = new EndPointInfo(_metaProvider.ServiceName,
                _metaProvider.GetEndPointName(selector.GetProperty()));
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

        public void Publish<TMessage, TStore>(Expression<Func<TService, IEndPoint<TMessage>>> selector, TStore store,
            TMessage message) where TStore : IDeliveryStore
        {
            var endPointInfo = new EndPointInfo(_metaProvider.ServiceName,
                _metaProvider.GetEndPointName(selector.GetProperty()));
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