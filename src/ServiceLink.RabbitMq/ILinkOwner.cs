using System;
using System.Threading.Tasks;
using RabbitLink.Configuration;
using RabbitLink.Consumer;
using RabbitLink.Producer;
using RabbitLink.Topology;
using ServiceLink.RabbitMq.Topology;

namespace ServiceLink.RabbitMq
{
    internal interface ILinkOwner
    {
        ILinkProducer GetOrAddProducer(ProducerParams @params);

        ILinkConsumer CreateConsumer(Func<ILinkTopologyConfig, Task<ILinkQueue>> topologyConfiguration, Action<ILinkConsumerConfigurationBuilder> config);
    }
}