using System;
using System.Collections.Concurrent;
using RabbitLink;
using RabbitLink.Producer;
using RabbitLink.Topology;

namespace ServiceLink.RabbitMq
{
    public class RabbitMqTransport : ITransport
    {
        private readonly Link _link;
        private readonly ISerializerSelector<byte[]> _serializerSelector;
        private readonly ConcurrentDictionary<(string, string), ILinkProducer> _producers = new ConcurrentDictionary<(string, string), ILinkProducer>();

        public RabbitMqTransport(Link link, ISerializerSelector<byte[]> serializerSelector)
        {
            _link = link;
            _serializerSelector = serializerSelector;
        }
        
        public IProducer<TMessage> GetOrAddProducer<TMessage>(EndPointInfo info)
        {
            var serializer = _serializerSelector.Select<TMessage>(info);
            var linkProducer = _producers.GetOrAdd((info.ServiceName, info.EndpointName), _ =>
                _link.CreateProducer(cfg => cfg.ExchangeDeclare(info.ServiceName, LinkExchangeType.Direct),
                    config: bld => bld.ConfirmsMode(true)));
            return new Producer<TMessage>(linkProducer, serializer, info.EndpointName);
        }
    }
}