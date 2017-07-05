using System;
using JetBrains.Annotations;
using ServiceLink.Metadata;
using ServiceLink.Serializers;

namespace ServiceLink.RabbitMq.Topology
{
    public class ProducerParams 
    {
        public ProducerParams(
            [NotNull] string exchangeName,
            bool confirmMode, [NotNull] PublishConfigure publishConfigure,
            [NotNull] ProducerConfigure configure) 
        {
            ExchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
            ConfirmMode = confirmMode;
            PublishConfigure = publishConfigure ?? throw new ArgumentNullException(nameof(publishConfigure));
            Configure = configure ?? throw new ArgumentNullException(nameof(configure));
        }

        [NotNull]
        public string ExchangeName { get; }
        
        public bool ConfirmMode { get; }
        
        [NotNull]
        public ProducerConfigure Configure { get; }
        
        [NotNull]
        public PublishConfigure PublishConfigure { get; }
    }
}