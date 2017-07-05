using System;
using RabbitLink.Topology;
using ServiceLink.RabbitMq.Topology;
using ServiceLink.Serializers;

namespace ServiceLink.RabbitMq.Configuration
{
    public class NotifyExchangeConfig : IExchangeConfig
    {
        public NotifyExchangeConfig(string exchangeName, bool confirmMode, LinkExchangeType exchangeType,
            TimeSpan? messageTtl, string routingKey)
        {
            ExchangeName = exchangeName;
            ConfirmMode = confirmMode;
            ExchangeType = exchangeType;
            MessageTtl = messageTtl;
            RoutingKey = routingKey;
        }

        public string ExchangeName { get; }
        public bool ConfirmMode { get; }
        public LinkExchangeType ExchangeType { get; }
        public string RoutingKey { get; }
        public TimeSpan? MessageTtl { get; }

        private PublishParams Apply(Serialized<byte[]> message)
        {
            var @params = new PublishParams();
            @params.ApplySerialization(message);
            @params.PublishProperties.RoutingKey = RoutingKey;
            if (MessageTtl != null)
                @params.MessageProperties.Expiration = MessageTtl;
            return @params;
        }

        public ProducerParams GetProducerParams()
        {
            return new ProducerParams(ExchangeName, ConfirmMode, 
                Apply, link => link.CreateProducer(cfg => cfg.ExchangeDeclare(ExchangeName, ExchangeType), config: cfg => cfg.ConfirmsMode(ConfirmMode)));
        }
    }
}