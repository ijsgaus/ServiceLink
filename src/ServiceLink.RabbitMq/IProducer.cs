using ServiceLink.Bus;

namespace ServiceLink.RabbitMq
{
    public interface IProducer
    {
        IPublisher<SerializedMessage> GetProducer();
        IBusSerializer GetSerializer();
    }
}