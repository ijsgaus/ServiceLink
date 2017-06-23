using System;

namespace ServiceLink
{
    public abstract class Envelope
    {
        private Envelope(string holderName)
        {
            HolderName = holderName;
        }
        
        public class Message : Envelope
        {
            public Message(string holderName, Guid? deliveryId) : base(holderName)
            {
                DeliveryId = deliveryId;
            }
            
            public Guid? DeliveryId { get; }
        }
        
        public class Answer : Envelope
        {
            public Answer(string holderName, Guid deliveryId) : base(holderName)
            {
                DeliveryId = deliveryId;
            }
            
            public Guid DeliveryId { get; }
        }
        
         public string HolderName { get; }   
    }
}