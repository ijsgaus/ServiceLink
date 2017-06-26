using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using RabbitLink.Consumer;
using RabbitLink.Messaging;
using ServiceLink.Monads;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    internal class InPoint<TMessage> : IInPoint<TMessage>
    {
        private readonly Func<ILinkConsumer> _consumerFactory;
        private readonly ISerializer<byte[]> _serializer;

        public InPoint(Func<ILinkConsumer> consumerFactory, ISerializer<byte[]> serializer)
        {
            _consumerFactory = consumerFactory;
            _serializer = serializer;
        }

        public IDisposable Subscribe(Func<Envelope, TMessage, CancellationToken, Task<AnswerKind>> subscriber, bool awaitSubscriber)
        {
            async Task Loop(ILinkConsumer consumer, CancellationToken token)
            {
                async Task ProcessMessage(ILinkMessage<byte[]> linkMessage)
                {
                    try
                    {
                        var deliveryId = Guid.TryParse(linkMessage.Properties.CorrelationId, out var guid)
                            ? (Guid?) guid
                            : null;
                        var answerTo = linkMessage.Properties.ReplyTo;
                        answerTo = string.IsNullOrWhiteSpace(answerTo) ? null : answerTo; 
                        var header = 
                            answerTo == null &&  deliveryId != null ? (Envelope) new Envelope.Answer(linkMessage.Properties.AppId, deliveryId.Value) 
                                : new Envelope.Message(linkMessage.Properties.AppId, deliveryId);
                        var msg = _serializer.TryDeserialize<TMessage>(new Serialized<TMessage>(
                            ContentType.Parse(linkMessage.Properties.ContentType),
                            EncodedType.Parse(linkMessage.Properties.Type),
                            linkMessage.Body)).Unwrap();
                        var result = await subscriber(header, msg, token);
                        switch (result)
                        {
                            case AnswerKind.Ack:
                                await linkMessage.AckAsync(token);
                                break;
                            case AnswerKind.Nack:
                                await linkMessage.NackAsync(token);
                                break;
                            case AnswerKind.Requeue:
                                await linkMessage.RequeueAsync(token);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    catch (Exception)
                    {
                        if (token.IsCancellationRequested) return;
                        throw;
                    }
                }
                
                while (true)
                {
                    ILinkMessage<byte[]> message;
                    try
                    {
                        message = await consumer.GetMessageAsync<byte[]>(token);
                    }
                    catch (Exception)
                    {
                        if (token.IsCancellationRequested) return;
                        throw;
                    }
                    if (token.IsCancellationRequested) return;
                    if (awaitSubscriber)
                        await ProcessMessage(message);
                    else
#pragma warning disable 4014
                        ProcessMessage(message);
#pragma warning restore 4014

                }

            }
            
            var cancellationDisposable = new CancellationDisposable();
            var linkConsumer = _consumerFactory();
            Loop(linkConsumer, cancellationDisposable.Token).ContinueWith(_ => linkConsumer.Dispose());
            return cancellationDisposable;
        }
    }
}