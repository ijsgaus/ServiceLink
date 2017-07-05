using System;
using RabbitLink.Messaging;
using ServiceLink.Serializers;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Channels
{
    internal class ReceiveContext : IRecieveContext<byte[]>
    {
        private readonly ILinkMessage<byte[]> _message;
        private readonly Lazy<Serialized<byte[]>> _msg; 
        
        public ReceiveContext(ILinkMessage<byte[]> message)
        {
            _message = message;
            _msg = new Lazy<Serialized<byte[]>>(() => new Serialized<byte[]>(
                ContentType.Parse(message.Properties.ContentType), EncodedType.Parse(message.Properties.Type), message.Body)); 
            
        }

        public Serialized<byte[]> Message => _msg.Value;
    }
}