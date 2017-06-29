using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using RabbitLink;
using RabbitLink.Configuration;
using RabbitLink.Consumer;
using RabbitLink.Messaging;
using RabbitLink.Producer;
using RabbitLink.Topology;
using ServiceLink.Metadata;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    public class RabbitMqTransport : ITranport, ILinkOwner
    {
        private readonly Link _link;
        private readonly ConcurrentDictionary<(string, bool), ILinkProducer> _producers = new ConcurrentDictionary<(string, bool), ILinkProducer>();
        private readonly IEndpointConfigure _configure;
        private readonly ILoggerFactory _loggerFactory;


        ILinkProducer ILinkOwner.GetOrAddProducer(string exchangeName, bool confirmMode, LinkExchangeType exchangeType)
        {
            return _producers.GetOrAdd((exchangeName, confirmMode), _link.CreateProducer(cfg =>
                    cfg.ExchangeDeclare(exchangeName, exchangeType),
                config: bld => bld.ConfirmsMode(confirmMode)));
        }


        public INotifyTransport<TMessage> EndPoint<TMessage>([NotNull] EndPointParams parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            return new NotifyTransport<TMessage>(_loggerFactory.CreateLogger<NotifyTransport<TMessage>>(),
                _configure, this, parameters);
        }

        ILinkConsumer ILinkOwner.CreateConsumer(Func<ILinkTopologyConfig, Task<ILinkQueue>> topologyConfiguration, Action<ILinkConsumerConfigurationBuilder> config)
            => _link.CreateConsumer(topologyConfiguration, config: config)
    }

    public interface IEndpointConfigure
    {
        EventParameters ConfigureEvent(EventParameters parameters, EndPointParams info);
    }

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Field | AttributeTargets.Property)]
    public class ExchangeNameAttribute : Attribute
    {
        public ExchangeNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
        
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RoutingKeyAttribute : Attribute
    {
        public RoutingKeyAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}