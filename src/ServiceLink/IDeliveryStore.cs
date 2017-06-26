using System;

namespace ServiceLink
{
    public interface IDeliveryStore<TMessage>
    {
        IDeliveryLeaseController<TMessage> CreateDelivery(EndPointInfo info, TMessage message);
        void AfterCommit(Action action);
    }
}