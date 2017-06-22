using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ServiceLink
{
    public interface IEndPoint<TMessage, TAnswer>
    {
        Guid Publish(IDeliveryStore store, TMessage message, TimeSpan? retryInterval = null);
        IObservable<TMessage> Published { get; }
    }

    public interface IEndPoint<TMessage> : IEndPoint
    {
        Task FireAsync(TMessage message, CancellationToken? token = null);
        Guid Publish(IDeliveryStore store, TMessage message, TimeSpan? retryInterval = null);
        IObservable<TMessage> Published { get; }
        IDisposable Subscibe(Action<IMessageHeader, TMessage> subscriber);
        //IDisposable Subscibe(Func<IMessageHeader, TMessage, CancellationToken, Task> subscriber);
    }

    public interface IEndPoint
    {
        [NotNull]
        EndPointInfo Info { get; }
    }
}