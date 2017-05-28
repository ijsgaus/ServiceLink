using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Bus
{
    public interface IPublisher
    {
        Task PublishAsync<T>(T message, CancellationToken? token = null);
        void PublishSafe<T>(T message, IPublishSafePoint safer);
    }

    public interface IPublisher<in T>
    {
        Task Publish(T message, CancellationToken? token = null);
    }
}