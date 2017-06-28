using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using RabbitLink;
using RabbitLink.Messaging;
using RabbitLink.Producer;
using RabbitLink.Topology;
using ServiceLink.Metadata;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    public class RabbitMqTransport : ITranport
    {
        private readonly Link _link;
        private readonly ConcurrentDictionary<string, ILinkProducer> _producers = new ConcurrentDictionary<string, ILinkProducer>();
        private readonly IReadOnlyCollection<IEndpointConfigure> _serviceConfigurers;
        private readonly IReadOnlyCollection<IEndpointConfigure> _endPointsConfigurers;
        private readonly ILoggerFactory _loggerFactory;

        public ITransportEventPoint<TMessage> EndPoint<TMessage>([NotNull] IHolder holder, [NotNull] EndPointParams parameters)
        {
            if (holder == null) throw new ArgumentNullException(nameof(holder));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            
            var evParam = new EventParameters(parameters.ServiceName, parameters.EndpointName, parameters.Serializer, 
                "{1}.{2}.{0}.{3}", "{1}.{2}.{0}", 1);
            evParam = _serviceConfigurers.Aggregate(evParam, (c, p) => p.ConfigureEvent(c, parameters));
            evParam = _endPointsConfigurers.Aggregate(evParam, (c, p) => p.ConfigureEvent(c, parameters));
            return new EventPoint<TMessage>(evParam.Serializer,
                _loggerFactory.CreateLogger<EventPoint<TMessage>>(),
                    serialized =>
                    {
                        var producer = _producers.GetOrAdd(evParam.ExchangeName,
                            _ => _link.CreateProducer(cfg =>
                                cfg.ExchangeDeclare(evParam.ExchangeName, LinkExchangeType.Direct)));
                        var properties = new LinkMessageProperties
                        {
                            AppId = holder.Name,
                            ContentType = serialized.ContentType.ToString(),
                            Type = serialized.Type.ToString(),
                            TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                        };
                        var pProp = new LinkPublishProperties
                        {
                            RoutingKey = evParam.RoutingKey
                        };
                        return token => producer.PublishAsync(serialized.Data, properties, pProp, token);
                    },
                separate =>
                {
                    var guid = Guid.NewGuid();
                    return _link.CreateConsumer(async cfg =>
                    {
                        var exchange = await cfg.ExchangeDeclarePassive(evParam.ExchangeName);
                        ILinkQueue queue;
                        if (separate)
                        {
                            queue = await cfg.QueueDeclare(
                                string.Format(evParam.TempQueueFormat, holder.Name, parameters.ServiceName,
                                    parameters.EndpointName,
                                    guid), expires: TimeSpan.FromMinutes(3));
                        }
                        else
                        {
                            queue = await cfg.QueueDeclare(
                                string.Format(evParam.QueueFormat, holder.Name, parameters.ServiceName,
                                    parameters.EndpointName));
                        }
                        await cfg.Bind(queue, exchange, evParam.RoutingKey);
                        return queue;
                    }, config: p => p.PrefetchCount(evParam.PrefetchCount));
                }
            );
        }

        /*
        public IBroadcastChannel<T> GetBroadcast<T>(EndPointInfo info, IHolder holder)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            var parameters = new BroadcastParameters(info.ServiceName, info.EndpointName, info.Serializer, 
                "{1}.{2}.{0}.{3}");
            parameters = _serviceConfigurers.Aggregate(parameters, (c, p) => p.ConfigureBroadcast(c, info));
            parameters = _endPointsConfigurers.Aggregate(parameters, (c, p) => p.ConfigureBroadcast(c, info));
            return new BroadcastChannel<T>(
                () =>
                {
                    var guid = Guid.NewGuid();
                    return _link.CreateConsumer(async cfg =>
                    {
                        var exchange = await cfg.ExchangeDeclarePassive(parameters.ExchangeName);
                        var queue = await cfg.QueueDeclare(
                            string.Format(parameters.QueueFormat, holder.Name, info.ServiceName, info.EndpointName,
                                guid), expires: TimeSpan.FromMinutes(3));
                        await cfg.Bind(queue, exchange, parameters.RoutingKey);
                        return queue;
                    }, config: p => p.AutoAck(true).PrefetchCount(10));
                }, parameters.Serializer, new Lazy<Action<ISerialized<byte[]>>>(() =>
                {
                    var producer = _producers.GetOrAdd(parameters.ExchangeName,
                        _ => _link.CreateProducer(cfg =>
                            cfg.ExchangeDeclare(parameters.ExchangeName, LinkExchangeType.Direct)));
                    return serialized =>
                    {
                        var properties = new LinkMessageProperties
                        {
                            AppId = holder.Name,
                            ContentType = serialized.ContentType.ToString(),
                            Type = serialized.Type.ToString(),
                            TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                        };
                        var pProp = new LinkPublishProperties
                        {
                            RoutingKey = parameters.RoutingKey
                        };
                        producer.PublishAsync(serialized.Data, properties, pProp);
                    };
                })
            );
        }
        */
          
        
    }

    public class EventParameters
    {
        public EventParameters(string exchangeName, string routingKey, ISerializer<byte[]> serializer, string tempQueueFormat, string queueFormat, ushort prefetchCount)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
            Serializer = serializer;
            TempQueueFormat = tempQueueFormat;
            QueueFormat = queueFormat;
            PrefetchCount = prefetchCount;
        }

        public string ExchangeName { get; }
        public string RoutingKey { get; }
        /// <summary>
        /// {0} - holder name
        /// {1} - service name
        /// {2} - endpointName
        /// {3} - guid
        /// </summary>
        public string TempQueueFormat { get; }
        
        /// <summary>
        /// {0} - holder name
        /// {1} - service name
        /// {2} - endpointName
        /// </summary>
        public string QueueFormat { get;  }
        
        public ISerializer<byte[]> Serializer { get; }
        public ushort PrefetchCount { get; }
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