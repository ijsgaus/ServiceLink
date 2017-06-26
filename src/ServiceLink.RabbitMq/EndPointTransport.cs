using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RabbitLink;
using RabbitLink.Consumer;
using RabbitLink.Messaging;
using RabbitLink.Producer;
using RabbitLink.Topology;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    internal class EndPointTransport<TMessage, TAnswer> : IEndPointTransport<TMessage, TAnswer>
    {
        


        public EndPointTransport([NotNull] OutPoint<TMessage> messageOutPoint,
            [NotNull] OutPoint<TAnswer> answerOutPoint,
            [NotNull] IInPoint<TMessage> messageInPoint,
            [NotNull] IInPoint<TAnswer> answerInPoint)
        {
            MessageInPoint = messageInPoint ?? throw new ArgumentNullException(nameof(messageInPoint));
            AnswerInPoint = answerInPoint ?? throw new ArgumentNullException(nameof(answerInPoint));
            MessageOutPoint = messageOutPoint ?? throw new ArgumentNullException(nameof(messageOutPoint));
            AnswerOutPoint = answerOutPoint ?? throw new ArgumentNullException(nameof(answerOutPoint));
            
            
        }


        public IOutPoint<TMessage> MessageOutPoint { get; }

        public IOutPoint<TAnswer> AnswerOutPoint { get; }

        public IInPoint<TMessage> MessageInPoint { get; }

        public IInPoint<TAnswer> AnswerInPoint { get; }
    }

    
}