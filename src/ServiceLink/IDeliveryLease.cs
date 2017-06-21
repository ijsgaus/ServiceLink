using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    public interface IDeliveryLease 
    {
        Guid DeliveryId { get; }
        bool Renew(TimeSpan? interval = null);
        Task WhenRenew(CancellationToken token);
        void RemoveDelivery();
    }
}