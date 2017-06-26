using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ServiceLink
{
    public interface ILinkEvent<TPayload>
    {
        Func<CancellationToken, Guid> Publish<TStore>([NotNull] IStakeHolder<TStore> holder, [CanBeNull] TStore store,
            TPayload payload) where TStore : IDeliveryStore;

        IDisposable Subscribe<TStore>([NotNull] IStakeHolder<TStore> holder,
            Func<TPayload, CancellationToken, Task> subscriber) where TStore : IDeliveryStore;

        IDisposable Listen<TStore>([NotNull] IStakeHolder<TStore> holder,
            Func<TPayload, CancellationToken, Task> subscriber) where TStore : IDeliveryStore;


        IObservable<EventPublishStateChanged<TPayload>> OnDelivered { get; }
    }

    public class EventPublishStateChanged<TPayload>
    {
        public EventPublishStateChanged(TPayload payload, Guid deliveryId)
        {
            Payload = payload;
            DeliveryId = deliveryId;
        }

        public TPayload Payload { get; }
        public Guid DeliveryId { get; }
    }

    public interface ILinkCall<TArgs, TResult>
    {
        Func<CancellationToken, Guid> Call<TStore>([NotNull] IStakeHolder<TStore> holder, [CanBeNull] TStore store,
            TArgs payload) where TStore : IDeliveryStore;
        
        IDisposable Subscribe<TStore>([NotNull] IStakeHolder<TStore> holder,
            Func<TArgs, CancellationToken, Task<TResult>> subscriber) where TStore : IDeliveryStore;
    }

}