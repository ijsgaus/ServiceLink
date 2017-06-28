using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Transport
{
    public interface ITransportEventPoint<TMessage>
    {
        Func<CancellationToken, Task> PrepareSend(TMessage message);
        IObservable<IAck<TMessage>> Connect(bool separate);
    }
}