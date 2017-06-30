using ServiceLink.Metadata;

namespace ServiceLink.RabbitMq.Topology
{
    public interface ITransportConfiguration
    {
        ProducerParams GetProducerConfig(EndPointParams endPoint);
    }
}