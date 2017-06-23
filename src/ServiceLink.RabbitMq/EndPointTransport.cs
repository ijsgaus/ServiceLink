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
        


        public EndPointTransport([NotNull] Producer<TMessage> messageProducer,
            [NotNull] Producer<TAnswer> answerProducer,
            [NotNull] IConsumer<TMessage> messageConsumer,
            [NotNull] IConsumer<TAnswer> answerConsumer)
        {
            MessageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
            AnswerConsumer = answerConsumer ?? throw new ArgumentNullException(nameof(answerConsumer));
            MessageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
            AnswerProducer = answerProducer ?? throw new ArgumentNullException(nameof(answerProducer));
            
            
        }


        public IProducer<TMessage> MessageProducer { get; }

        public IProducer<TAnswer> AnswerProducer { get; }

        public IConsumer<TMessage> MessageConsumer { get; }

        public IConsumer<TAnswer> AnswerConsumer { get; }
    }

    
}