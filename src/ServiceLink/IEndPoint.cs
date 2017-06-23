using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ServiceLink
{
    public interface IEndPoint<TMessage, TAnswer>
    {
        Guid Publish(IDeliveryStore<TMessage> store, TMessage message, TimeSpan? retryInterval = null);
        IObservable<TMessage> Published { get; }
    }

    public interface IEndPoint<TMessage> : IEndPoint
    {
        Task FireAsync(TMessage message, CancellationToken? token = null);
        Guid Publish(IDeliveryStore<TMessage> store, TMessage message, TimeSpan? retryInterval = null);
        IObservable<TMessage> Published { get; }
        IDisposable Subscibe(Func<Envelope, TMessage, Answer<ValueTuple>> subscriber);
        //IDisposable Subscibe(Func<IMessageHeader, TMessage, CancellationToken, Task> subscriber);
    }

    public interface IEndPoint
    {
        [NotNull]
        EndPointInfo Info { get; }
    }
}