using System.Threading.Tasks;

namespace ServiceLink
{
    public interface IConsumer<in TMessage>
    {
        Task Consume(IConsumeContext<TMessage> context);
    }

    public interface IConsumeContext<out TMessage>
    {
        TMessage Message { get; }
    }
}