using System;
using System.Threading;
using System.Threading.Tasks;
using ServiceLink.Serializers;

namespace ServiceLink.Transport
{
    public interface ITransportOutChannel<TFormat>
    {
        Func<CancellationToken, Task> GetSender(Serialized<TFormat> message);
    }
}