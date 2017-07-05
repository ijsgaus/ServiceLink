using ServiceLink.RabbitMq.Topology;

namespace ServiceLink.RabbitMq.Configuration
{
    public interface IExchangeConfig
    {
        ProducerParams GetProducerParams();
    }
}