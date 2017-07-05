using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink;
using RabbitLink.Producer;
using RabbitLink.Topology;
using ServiceLink.RabbitMq.Topology;
using ServiceLink.Serializers;

namespace ServiceLink.RabbitMq
{
    internal static class Produce
    {

        public static ILinkProducer CommonProducerConfigure(Link link, ProducerParams @params)
            => link.CreateProducer(cfg => cfg.ExchangeDeclare(@params.ExchangeName, LinkExchangeType.Direct),
                config: bld => bld.ConfirmsMode(@params.ConfirmMode));

        
        
        public static Func<TMessage, Func<CancellationToken, Task>> PrepareSend<TMessage>(ILogger logger,
            ISerializer<byte[]> serializer,
            Lazy<ILinkProducer> producerLazy, Func<ProduceParams, ProduceParams> paramsCorrector)
            => message =>
            {
                var serialized = serializer.Serialize(message);
                var param = new ProduceParams();
                param.MessageProperties.ContentType = serialized.ContentType.ToString();
                param.MessageProperties.Type = serialized.Type.ToString();
                param.MessageProperties.TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                param = paramsCorrector(param);
                var producer = producerLazy.Value;
                return ct =>
                {
                    var task = producer.PublishAsync(serialized.Data, param.MessageProperties, param.PublishProperties,
                        ct);
                    task.ContinueWith(OnPublishFinished(message, logger), CancellationToken.None);
                    return task;
                };
            };

        public static Func<ILinkProducer> ProducerFactory(this ILinkOwner owner, string exchangeName, bool confirmMode,
            LinkExchangeType exchangeType)
            => () => owner.GetOrAddProducer(exchangeName, confirmMode, exchangeType);
    }

}