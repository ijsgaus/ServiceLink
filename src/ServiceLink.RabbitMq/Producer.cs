using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RabbitLink.Messaging;
using RabbitLink.Producer;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
     
    
    internal class Producer<TMessage> : IProducer<TMessage>
    {
        private readonly Func<TMessage, PublishParameters, (LinkMessageProperties, LinkPublishProperties)> _propertyFactory;
        private readonly Lazy<ILinkProducer> _producer;
        private readonly ISerializer<byte[]> _serializer;
        

        public Producer([NotNull] Func<ILinkProducer> producerFactory, [NotNull] Func<TMessage, PublishParameters, (LinkMessageProperties, LinkPublishProperties)> propertyFactory,
            [NotNull] ISerializer<byte[]> serializer)
        {
            _propertyFactory = propertyFactory ?? throw new ArgumentNullException(nameof(propertyFactory));
            if (producerFactory == null) throw new ArgumentNullException(nameof(producerFactory));
            
            _producer = new Lazy<ILinkProducer>(producerFactory);
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            
        }

        public Func<CancellationToken, Task> Publish(TMessage message, PublishParameters parameters)
        {
            var msg = _serializer.Serialize(message);
            var (msgProp, pubProp) = _propertyFactory(message, parameters);
            var producer = _producer.Value;
            return token => producer.PublishAsync(msg.Data, msgProp, pubProp, token);
        }
    }
}