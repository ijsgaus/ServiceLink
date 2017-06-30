using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Transport
{
    public interface INotifyTransport<TMessage>
    {
        Func<CancellationToken, Task> PrepareSend(TMessage message);
        IObservable<IAck<TMessage>> Connect(ReceiveParams @params);
    }
}