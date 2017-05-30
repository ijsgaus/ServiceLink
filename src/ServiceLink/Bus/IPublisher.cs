using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Bus
{
    public interface IPublisher
    {
        Task PublishAsync<T>(T message, CancellationToken? token = null);
        Action PublishSafe<T>(T message, IPublishSafePoint safer);
    }

    public interface IPublisher<in T>
    {
        Func<CancellationToken, Task> PreparePublish(T message);
    }
}