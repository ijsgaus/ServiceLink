using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    public interface IDeliveryLeaseController<out TMessage> 
    {
        Guid DeliveryId { get; }
        TMessage Message { get; }
        bool Renew(TimeSpan? interval = null);
        Task WhenRenew(CancellationToken token);
        void RemoveDelivery();
    }
}