using RabbitLink;
using RabbitLink.Producer;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Topology
{
    public delegate ILinkProducer ProducerConfigure(Link link, ProducerParams @params);

    public delegate PublishParams PublishConfigure<in TMessage>(TMessage message, ProducerParams @params,
        SendParams sendParams);
}