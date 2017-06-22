using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Transport
{
    public interface IEndPointTransport<TMessage, TAnswer>
    {
        IProducer<TMessage> MessageProducer { get; }
        IProducer<TAnswer> AnswerProducer { get; }
        IConsumer<TMessage> MessageConsumer { get; }
        IConsumer<TAnswer> AnswerConsumer { get; }
    }
}