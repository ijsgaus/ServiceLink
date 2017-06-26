using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RabbitLink.Messaging;
using RabbitLink.Producer;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
     
    
    internal class OutPoint<TMessage> : IOutPoint<TMessage>
    {
        private readonly Func<TMessage, SendParams, (LinkMessageProperties, LinkPublishProperties)> _propertyFactory;
        private readonly Lazy<ILinkProducer> _producer;
        private readonly ISerializer<byte[]> _serializer;
        

        public OutPoint([NotNull] Func<ILinkProducer> producerFactory, [NotNull] Func<TMessage, SendParams, (LinkMessageProperties, LinkPublishProperties)> propertyFactory,
            [NotNull] ISerializer<byte[]> serializer)
        {
            _propertyFactory = propertyFactory ?? throw new ArgumentNullException(nameof(propertyFactory));
            if (producerFactory == null) throw new ArgumentNullException(nameof(producerFactory));
            
            _producer = new Lazy<ILinkProducer>(producerFactory);
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            
        }

        public Func<CancellationToken, Task> Publish(TMessage message, SendParams @params)
        {
            var msg = _serializer.Serialize(message);
            var (msgProp, pubProp) = _propertyFactory(message, @params);
            var producer = _producer.Value;
            return token => producer.PublishAsync(msg.Data, msgProp, pubProp, token);
        }
    }
}