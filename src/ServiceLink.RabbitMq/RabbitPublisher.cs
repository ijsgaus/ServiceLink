using System;
using System.Threading;
using System.Threading.Tasks;
using ServiceLink.Bus;
using ServiceLink.Exceptions;
using ServiceLink.Monads;

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

        public Func<CancellationToken, Task> PreparePublish(T message)
        {
            var producer = _resolver.GetTopology(message);
            if (producer == null)
                throw new ServiceLinkException($"Producer for {message} not detected");
            var serialized = producer.GetSerializer().Serialize(message).Unwrap();
            return producer.GetProducer().PreparePublish(serialized);
        }
    }
}