using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    public interface ICommandPoint<T>
    {
        IHolder Holder { get; }
        void SendAndForget(T command);
        Task SendAsync(T command, CancellationToken? token = null);

        IObservable<IAck<T>> Recieve();
    }

    public interface ICommandPoint<TCommand, TStore> : ICommandPoint<TCommand>
        where TStore : IDeliveryStore
    {
        IStoreHolder<TStore> StoreHolder { get; }
        Action GetExecutor(TStore store, TCommand command, TimeSpan? timeout = null);
    }
}