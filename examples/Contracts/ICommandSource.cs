
using System;

namespace Contracts
{
    public interface ICommandSource
    {
        void Execute(Command command);

        Func<Command, int> Exec { get; }
    }

    public class Command
    {
    }
}