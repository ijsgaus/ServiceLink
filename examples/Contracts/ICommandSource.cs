
using System;

namespace Contracts
{
    public interface ICommandSource
    {
        EventHandler<SampleEvent> Sample { get; }
        
        
        void Execute(Command command);

        int Exec(Command command);
        Func<Command, int> SampleWithAnswer { get; }
    }

    public class Command
    {
    }

    public class SampleEvent
    {
        
    }
}