

using System;
using RabbitLink;
using RabbitLink.Consumer;
using ServiceLink.Endpoints;
using ServiceLink.RabbitMq.Configuration;

namespace ServiceLink.RabbitMq.Topology
{
    public interface ITransportConfiguration
    {
        IExchangeConfig GetNotifyExchangeConfig(NotifyEndpoint endpoint);
        Func<Link, ILinkConsumer> GetNotifyQueueFactory(NotifyEndpoint endpoint);
    }
}