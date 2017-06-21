using System;

namespace ServiceLink
{
    public interface IDeliveryStore
    {
        IDeliveryLease Save<TMessage>(EndPointInfo info, TMessage message);
        void AfterCommit(Action action);
    }
}