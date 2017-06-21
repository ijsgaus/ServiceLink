using ServiceLink;

namespace Contracts
{
    public interface ICommandSource
    {
        IEndPoint<Command> Execute { get; }
    }

    public class Command
    {
    }
}