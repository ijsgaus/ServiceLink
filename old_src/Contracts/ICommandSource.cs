
using System;
using ServiceLink.Markers;

namespace Contracts
{
    public interface ICommandSource
    {
        INotify<SampleEvent> Sample { get; }
        
        
        ICallable<Command, int> SampleWithAnswer { get; }
    }

    public class Command
    {
    }

    public class SampleEvent
    {
        
    }
}