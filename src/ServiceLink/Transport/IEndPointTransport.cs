using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Transport
{
    public interface IEndPointTransport<TMessage, TAnswer>
    {
        IOutPoint<TMessage> MessageOutPoint { get; }
        IOutPoint<TAnswer> AnswerOutPoint { get; }
        IInPoint<TMessage> MessageInPoint { get; }
        IInPoint<TAnswer> AnswerInPoint { get; }
    }
}