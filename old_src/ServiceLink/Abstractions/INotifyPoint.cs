using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    public interface INotifyPoint<TMessage, TStore> : INotifyPoint<TMessage>
        where TStore : IDeliveryStore
    {
        IStoreHolder<TStore> StoreHolder { get; }
        
        Action GetPublisher(TStore store, TMessage message, TimeSpan? timeout = null);
    }

    public interface INotifyPoint<TMessage>
    {
        IHolder Holder { get; }
        
        Task FireAsync(TMessage message, CancellationToken? token = null);
        
        IDisposable Listen(IConsumer<TMessage> consumer, IExecutor executor = null, IListenOptions options = null);
    }

    public interface IListenOptions
    {
    }
}