using System;

namespace ServiceLink.Transport
{
    public class PublishParameters
    {
        public PublishParameters(string holderName, Guid? deliveryId, bool needAnswer)
        {
            HolderName = holderName;
            DeliveryId = deliveryId;
            NeedAnswer = needAnswer;
        }

        public string HolderName { get; }
        public Guid? DeliveryId { get; }
        public bool NeedAnswer { get; }
    }
}