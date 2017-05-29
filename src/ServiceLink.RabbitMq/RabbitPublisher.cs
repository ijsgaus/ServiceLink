using System.Threading;
using System.Threading.Tasks;
using ServiceLink.Bus;
using ServiceLink.Exceptions;

namespace ServiceLink.RabbitMq
{
    public class RabbitPublisher<T> : IPublisher<T>
        where T : class
    {
        private readonly IProducerResolver _resolver;

        public RabbitPublisher(IProducerResolver resolver)
        {
            _resolver = resolver;
        }

        public Task Publish(T message, CancellationToken? token = null)
        {
            var producer = _resolver.GetTopology(message);
            if (producer == null)
                throw new ServiceLinkException($"Producer for {message} not detected");
            var serialized = producer.GetSerializer().Serialize(message);
            if (serialized == null)
                throw new ServiceLinkException($"Cannot serialize message {message}");
            return producer.GetProducer().Publish(serialized);
        }
    }
}