using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Consumer;
using RabbitLink.Producer;
using RabbitLink.Topology;
using ServiceLink.Metadata;
using ServiceLink.RabbitMq.Channels;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    internal class NotifyTransport<TMessage> : INotifyTransport<TMessage>
    {
        private readonly ILogger<NotifyTransport<TMessage>> _logger;
        private readonly Func<bool, IObservable<IAck<TMessage>>> _consume;
        private readonly ProduceChannel<TMessage> _produceChannel;

        public NotifyTransport(ILogger<NotifyTransport<TMessage>> logger, IEndpointConfigure configure,
            ILinkOwner owner, EndPointParams endPoint)
        {
            _logger = logger;
            var prm = new EventParameters(endPoint.ServiceName, endPoint.EndpointName, endPoint.Serializer, 
                "{1}.{2}.{0}.{3}", "{1}.{2}.{0}", 1, true);
            prm = configure.ConfigureEvent(prm, endPoint);
            _consume = Consume.MakeConnect<TMessage>(logger, prm.Serializer, 
                
                
                separate =>
                {
                    var guid = Guid.NewGuid();
                    return owner.CreateConsumer(async cfg =>
                    {
                        var exchange = await cfg.ExchangeDeclarePassive(prm.ExchangeName);
                        ILinkQueue queue;
                        if (separate)
                        {
                            queue = await cfg.QueueDeclare(
                                string.Format(prm.TempQueueFormat, endPoint.HolderName, endPoint.ServiceName,
                                    endPoint.EndpointName, guid), expires: TimeSpan.FromMinutes(3));
                        }
                        else
                        {
                            queue = await cfg.QueueDeclare(
                                string.Format(prm.QueueFormat, endPoint.HolderName, endPoint.ServiceName,
                                    endPoint.EndpointName));
                        }
                        await cfg.Bind(queue, exchange, prm.RoutingKey);
                        return queue;
                    }, p => p.PrefetchCount(prm.PrefetchCount));
                });
            _produceChannel = new ProduceChannel<TMessage>(logger, prm.Serializer, 
                new Lazy<ILinkProducer>(() => owner.GetOrAddProducer(prm.ExchangeName, prm.ProducerConfirmMode, LinkExchangeType.Direct)),
                pp =>
                {
                    pp.MessageProperties.AppId = endPoint.HolderName;
                    pp.PublishProperties.RoutingKey = prm.RoutingKey;
                    return pp;
                });
        }

        public Func<CancellationToken, Task> PrepareSend(TMessage message)
            => _produceChannel.PrepareSend(message);

        public IObservable<IAck<TMessage>> Connect(bool separate) => _consume(separate);
    }
}
