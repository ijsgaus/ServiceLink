using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    public interface IEventPoint<TMessage, TStore> : IEventPoint<TMessage>
        where TStore : IDeliveryStore
    {
        IStoreHolder<TStore> StoreHolder { get; }
        
        Action GetPublisher(TStore store, TMessage message, TimeSpan? timeout = null);
    }

    public interface IEventPoint<T>
    {
        IHolder Holder { get; }
        
        void FireAndForget(T message);

        Task FireAsync(T message, CancellationToken? token = null);
        
        IObservable<IAck<T>> Listen();
        
        IObservable<IAck<T>> Connect();
    }
}