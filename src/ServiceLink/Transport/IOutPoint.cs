using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Transport
{
    public interface IOutPoint<in TMessage>
    {
        Func<CancellationToken, Task> Publish(TMessage message, SendParams @params);
    }
}