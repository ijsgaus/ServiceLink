using System;
using System.Threading.Tasks;
using RabbitLink.Configuration;
using RabbitLink.Consumer;
using RabbitLink.Producer;
using RabbitLink.Topology;

namespace ServiceLink.RabbitMq
{
    internal interface ILinkOwner
    {
        ILinkProducer GetOrAddProducer(string exchangeName, bool confirmMode,
            LinkExchangeType exchangeType);

        ILinkConsumer CreateConsumer(Func<ILinkTopologyConfig, Task<ILinkQueue>> topologyConfiguration, Action<ILinkConsumerConfigurationBuilder> config);
    }
}