using System;

namespace ServiceLink
{
    public interface IDeliveryStore
    {
        IDeliveryLeaseController<TMessage> CreateDelivery<TMessage>(EndPointInfo info, TMessage message);
        void AfterCommit(Action action);
    }
}