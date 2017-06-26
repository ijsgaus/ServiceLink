using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Transport
{
    public interface IInPoint<out TMessage>
    {
        IDisposable Subscribe(Func<Envelope, TMessage, CancellationToken, Task<AnswerKind>> subscriber, bool awaitSubscriber);
    }
}