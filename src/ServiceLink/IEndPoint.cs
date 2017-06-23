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
        IDisposable Subscibe(Func<Envelope, TMessage, Answer<TAnswer>> subscriber);
        IDisposable Subscibe(Func<Envelope, TMessage, CancellationToken, Task<Answer<TAnswer>>> subscriber);
    }

    public interface IEndPoint<TMessage> : IEndPoint
    {
        Task FireAsync(TMessage message, CancellationToken? token = null);
        Guid Publish(IDeliveryStore<TMessage> store, TMessage message, TimeSpan? retryInterval = null);
        IObservable<TMessage> Published { get; }
        IDisposable Subscibe(Func<Envelope, TMessage, Answer<ValueTuple>> subscriber);
        IDisposable Subscibe(Func<Envelope, TMessage, CancellationToken, Task<Answer<ValueTuple>>> subscriber);
    }

    public interface IEndPoint
    {
        [NotNull]
        EndPointInfo Info { get; }
    }
}