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
using ServiceLink.RabbitMq.Topology;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    public class RabbitMqTransport : ITranport, ILinkOwner
    {
        private readonly Link _link;
        private readonly ConcurrentDictionary<(string, bool), ILinkProducer> _producers = new ConcurrentDictionary<(string, bool), ILinkProducer>();
        private readonly ITransportConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;


        ILinkProducer ILinkOwner.GetOrAddProducer(ProducerParams @params)
        {
            return _producers.GetOrAdd((@params.ExchangeName, @params.ConfirmMode), 
                _ => @params.Configure(_link, @params));
        }


        public INotifyTransport<TMessage> EndPoint<TMessage>([NotNull] EndPointParams parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            return new NotifyTransport<TMessage>(_loggerFactory.CreateLogger<NotifyTransport<TMessage>>(),
                _configure, this, parameters);
        }

        ILinkConsumer ILinkOwner.CreateConsumer(Func<ILinkTopologyConfig, Task<ILinkQueue>> topologyConfiguration,
            Action<ILinkConsumerConfigurationBuilder> config)
            => _link.CreateConsumer(topologyConfiguration, config: config);
    }

    

    
}