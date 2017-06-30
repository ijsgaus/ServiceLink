using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Transport
{
    public interface ICommandTransport<TCommand>
    {
        Func<CancellationToken, Task> PrepareSend(TCommand message, SendParams sendParams);
        IObservable<IAck<TCommand>> Connect();
        IObservable<IAck<Unit>> GetAnswer();
    }
}