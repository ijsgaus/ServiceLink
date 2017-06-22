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
    internal class Consumer<TMessage> : IConsumer<TMessage>
    {
        private readonly Func<ILinkConsumer> _consumerFactory;
        private readonly ISerializer<byte[]> _serializer;

        public Consumer(Func<ILinkConsumer> consumerFactory, ISerializer<byte[]> serializer)
        {
            _consumerFactory = consumerFactory;
            _serializer = serializer;
        }

        public IDisposable Subscribe(Func<IMessageHeader, TMessage, CancellationToken, Task<Acknowledge>> subscriber, bool awaitSubscriber)
        {
            async Task Loop(ILinkConsumer consumer, CancellationToken token)
            {
                async Task ProcessMessage(ILinkMessage<byte[]> linkMessage)
                {
                    try
                    {
                        var header = new MessageHeader();
                        var msg = _serializer.TryDeserialize<TMessage>(new Serialized<TMessage>(
                            ContentType.Parse(linkMessage.Properties.ContentType),
                            EncodedType.Parse(linkMessage.Properties.Type),
                            linkMessage.Body)).Unwrap();
                        var result = await subscriber(header, msg, token);
                        switch (result)
                        {
                            case Acknowledge.Ack:
                                await linkMessage.AckAsync(token);
                                break;
                            case Acknowledge.Nack:
                                await linkMessage.NackAsync(token);
                                break;
                            case Acknowledge.Requeue:
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