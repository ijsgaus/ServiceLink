using System.Threading.Tasks;

namespace ServiceLink.RabbitMq
{
    public interface IProducerResolver
    {
        IProducer GetTopology<T>(T message);
    }
}