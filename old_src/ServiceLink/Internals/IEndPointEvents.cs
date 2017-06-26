using System.Reactive.Subjects;

namespace ServiceLink
{
    internal interface IEndPointEvents<TMessage>
    {
        ISubject<TMessage, TMessage> Published { get; }
    }
}