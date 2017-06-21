using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    public interface IProducer<in TMessage>
    {
        Task Publish(TMessage message, IMessageSource source, CancellationToken token);
    }
}