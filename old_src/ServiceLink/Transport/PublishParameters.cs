using System;

namespace ServiceLink.Transport
{
    public class SendParams
    {
        public string HolderName { get; }
        
        private SendParams(string holderName)
        {
            
        }
        
        public sealed class WhenPublish : SendParams
        {
            public WhenPublish(string holderName, Guid? deliveryId) : base(holderName)
            {
                DeliveryId = deliveryId;
            }
            
            public Guid? DeliveryId { get; }
        }
        
        public sealed class WhenAnswer : SendParams
        {
            public WhenAnswer(string holderName, Guid deliveryId) : base(holderName)
            {
                DeliveryId = deliveryId;
            }
            
            public Guid DeliveryId { get; }
            
        }

        
        
        
    }
}