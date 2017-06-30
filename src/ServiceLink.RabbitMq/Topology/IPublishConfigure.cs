using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Topology
{
    public interface IPublishConfigure
    {
        PublishParams Configure<TMessage>(TMessage message, ProducerParams @params,
            SendParams sendParams);
    }
}