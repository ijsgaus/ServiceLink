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
using ServiceLink.Endpoints;
using ServiceLink.Metadata;
using ServiceLink.RabbitMq.Channels;
using ServiceLink.RabbitMq.Topology;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    public class RabbitMqTransport : ITransport<byte[]>, ILinkOwner
    {
        private readonly Link _link;
        private readonly ConcurrentDictionary<(string, bool), ILinkProducer> _producers = new ConcurrentDictionary<(string, bool), ILinkProducer>();
        private readonly ITransportConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;


        public ITransportOutChannel<byte[]> GetOutChannel([NotNull] Endpoint endpoint)
        {
            if (endpoint == null) throw new ArgumentNullException(nameof(endpoint));
            switch (endpoint)
            {
                case NotifyEndpoint ne:
                    var nex = _configuration.GetNotifyExchangeConfig(ne);
                    return new TransportOutChannel(_loggerFactory.CreateLogger<TransportOutChannel>(), this,
                        nex.GetProducerParams());
                default:
                    throw new ArgumentOutOfRangeException($"Unknown endpoint type {endpoint.GetType()}");
            }
        }

        public ITransportInChannel<byte[]> GetInChannel([NotNull] Endpoint endpoint)
        {
            if (endpoint == null) throw new ArgumentNullException(nameof(endpoint));
            switch (endpoint)
            {
                case NotifyEndpoint ne:
                    var factory = _configuration.GetNotifyQueueFactory(ne);
                    return new TransportInChannel(_loggerFactory.CreateLogger<TransportInChannel>(), CreateConsumer(factory));
                default:
                    throw new ArgumentOutOfRangeException($"Unknown endpoint type {endpoint.GetType()}");
            }
        }


        ILinkProducer ILinkOwner.GetOrAddProducer(ProducerParams @params)
        {
            return _producers.GetOrAdd((@params.ExchangeName, @params.ConfirmMode), 
                _ => @params.Configure(_link));
        }


        private Func<ILinkConsumer> CreateConsumer(Func<Link, ILinkConsumer> create)
            => () => create(_link);
    }

    

    
}