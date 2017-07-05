using RabbitLink;
using RabbitLink.Consumer;
using RabbitLink.Producer;
using ServiceLink.Serializers;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Topology
{
    public delegate ILinkProducer ProducerConfigure(Link link);

    public delegate PublishParams PublishConfigure(Serialized<byte[]> message);

    public delegate ILinkConsumer CreateConsumer(Link link);
}