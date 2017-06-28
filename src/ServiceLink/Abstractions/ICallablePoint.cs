using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    public interface ICallablePoint<TArgs, TResult>
    {
        IHolder Holder { get; }
        Task<TResult> CallAsync(TArgs args, CancellationToken? token);
        IObservable<IJob<TArgs, TResult>> Execute();
    }

    public interface ICallablePoint<TArgs, TResult, TStore> : ICallablePoint<TArgs, TResult>
        where TStore : IDeliveryStore
    {
        IStoreHolder<TStore> StoreHolder { get; }
        Action GetSender(TStore store, TArgs message, TimeSpan? timeout = null);
        IObservable<IAnswer<TArgs, TResult, TStore>> Receive();

    }
}