using System;

namespace ServiceLink
{
    public interface IDeliveryStore<TMessage>
    {
        IDeliveryLease<TMessage> CreateDelivery(EndPointInfo info, TMessage message);
        void AfterCommit(Action action);
    }
}