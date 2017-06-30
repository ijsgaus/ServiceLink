using System;
using JetBrains.Annotations;
using ServiceLink.Metadata;

namespace ServiceLink.RabbitMq.Topology
{
    public class ProducerParams : ChannelParams
    {
        public ProducerParams([NotNull] EndPointParams endPoint, [NotNull] ISerializer<byte[]> serializer,
            [NotNull] string exchangeName,
            [CanBeNull] string routingKey, bool confirmMode, [NotNull] IPublishConfigure publishConfigure,
            ProducerConfigure configure = null) : base(endPoint, serializer)
        {

            ExchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
            RoutingKey = routingKey;
            ConfirmMode = confirmMode;
            PublishConfigure = publishConfigure ?? throw new ArgumentNullException(nameof(publishConfigure));
            Configure = configure ?? Produce.CommonProducerConfigure;
        }

        [NotNull]
        public string ExchangeName { get; }
        
        public bool ConfirmMode { get; }
        
        [CanBeNull]
        public string RoutingKey { get; }
        
        [NotNull]
        public ProducerConfigure Configure { get; }
        
        [NotNull]
        public IPublishConfigure PublishConfigure { get; }
    }
}