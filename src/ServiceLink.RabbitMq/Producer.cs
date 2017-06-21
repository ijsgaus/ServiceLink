using System.Threading;
using System.Threading.Tasks;
using RabbitLink.Messaging;
using RabbitLink.Producer;

namespace ServiceLink.RabbitMq
{
    internal class Producer<TMessage> : IProducer<TMessage>
    {
        private readonly ILinkProducer _producer;
        private readonly ISerializer<byte[]> _serializer;
        private readonly string _routingKey;

        public Producer(ILinkProducer producer, ISerializer<byte[]> serializer, string routingKey)
        {
            _producer = producer;
            _serializer = serializer;
            _routingKey = routingKey;
        }

        public Task Publish(TMessage message, IMessageSource source, CancellationToken token)
        {
            var msg = _serializer.Serialize(message);
            var messageProps = new LinkMessageProperties
            {
                AppId = source.Application,
                ContentType = msg.ContentType.ToString(),
                Type = msg.Type.ToString()
            };
            var publishProps = new LinkPublishProperties
            {
                RoutingKey = _routingKey
            };
            return _producer.PublishAsync(msg.Data, messageProps, publishProps, token);
        }
    }
}