using ServiceLink;

namespace Contracts
{
    public interface ICommandSource
    {
        IEndPoint<Command> Execute { get; }

        void Exec(Command command);
    }

    public class Command
    {
    }
}