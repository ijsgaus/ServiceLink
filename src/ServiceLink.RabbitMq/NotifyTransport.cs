using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Consumer;
using RabbitLink.Producer;
using RabbitLink.Topology;
using ServiceLink.Metadata;
using ServiceLink.RabbitMq.Channels;
using ServiceLink.RabbitMq.Topology;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    internal class NotifyPoint<TMessage> : INotifyPoint<TMessage>
    {
        private readonly ProducerParams _producerParams;
        private readonly Lazy<ILinkProducer> _lazyProducer;
        
        public NotifyPoint(ILogger logger, ITransportConfiguration configuration,
            ILinkOwner owner, EndPointParams endPoint)
        {
            _logger = logger;
            _producerParams = configuration.GetProducerConfig(endPoint);
            _lazyProducer = new Lazy<ILinkProducer>(() => owner.GetOrAddProducer(_producerParams));
            Holder = endPoint.Holder;
            /*var prm = new EventParameters(endPoint.ServiceName, endPoint.EndpointName, endPoint.Serializer, 
                "{1}.{2}.{0}.{3}", "{1}.{2}.{0}", 1, true);
            prm = configure.ConfigureEvent(prm, endPoint);*/

            //var factory = ConsumerFactory(owner, prm, endPoint);

            // _consume = MakeConsume(logger, prm.Serializer, factory);

            //_produce = Produce.PrepareSend<TMessage>(logger, prm.Serializer, 
            //    new Lazy<ILinkProducer>(owner.ProducerFactory(prm.ExchangeName, prm.ProducerConfirmMode, LinkExchangeType.Direct)),
            //    pp =>
            //    {
            //        pp.MessageProperties.AppId = endPoint.HolderName;
            //        pp.PublishProperties.RoutingKey = prm.RoutingKey;
            //        return pp;
            //    });
        }


        public IHolder Holder { get; }

        public Task FireAsync(TMessage message, CancellationToken? token = null)
        {
            var tkn = token ?? CancellationToken.None;
            var log = _logger.With("@channel", _producerParams).With("@message", message);
            Func<CancellationToken, Task> Prepare()
            {
                var producer = _lazyProducer.Value;
                var serialized = _producerParams.Serializer.Serialize(message);
                var publishParams = _producerParams.PublishConfigure.Configure(message, _producerParams, null).ApplySerialization(serialized);
                
                Task Publish(CancellationToken cancellation)
                {
                    var task = producer.PublishAsync(serialized.Data, publishParams.MessageProperties,
                        publishParams.PublishProperties, cancellation);
                    task.LogResult(log, LogEvents.Publish);
                    return task;
                }

                return Publish;
            }

            return log.WithLog(Prepare, LogEvents.PreparePublish)(tkn);
        }

        public IObservable<IAck<TMessage>> Connect(bool separate) => _consume(separate);
        
        
        private readonly ILogger _logger;
        

        /*
        private static Func<bool, ILinkConsumer> ConsumerFactory(ILinkOwner owner, EventParameters prm,
            EndPointParams endPoint)
            => separate =>
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
            };
        
        private static Func<bool, IObservable<IAck<TMessage>>> MakeConsume(ILogger logger, ISerializer<byte[]> serializer, Func<bool, ILinkConsumer> factory)
            => separate => Consume.MakeConnect<TMessage>(logger, serializer, 
                () => factory(separate), Consume.ConfirmByAck);
        */
    }
}
