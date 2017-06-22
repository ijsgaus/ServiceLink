using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Transport
{
    public interface IProducer<in TMessage>
    {
        Func<CancellationToken, Task> Publish(TMessage message, PublishParameters parameters);
    }
}