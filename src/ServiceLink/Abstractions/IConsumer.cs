using System.Threading.Tasks;

namespace ServiceLink
{
    public interface IConsumer<in TMessage>
    {
        Task Consume(TMessage message, IConsumeContext context);
    }

    public interface IConsumeContext
    {
        
    }
}