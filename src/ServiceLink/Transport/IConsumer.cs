using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Transport
{
    public interface IConsumer<out TMessage>
    {
        IDisposable Subscribe(Func<IMessageHeader, TMessage, CancellationToken, Task<Acknowledge>> subscriber, bool awaitSubscriber);
    }
}