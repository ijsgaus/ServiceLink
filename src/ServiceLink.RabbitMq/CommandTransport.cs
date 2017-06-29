using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    public class CommandTransport<TCommand> : ICommandTransport<TCommand>
    {
        public Func<CancellationToken, Task> PrepareSend(TCommand message)
        {
            throw new NotImplementedException();
        }

        public IObservable<IAck<TCommand>> Connect()
        {
            throw new NotImplementedException();
        }

        public IObservable<IAck<Unit>> GetAnswer()
        {
            throw new NotImplementedException();
        }
    }
}