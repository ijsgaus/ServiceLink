using System;
using RabbitLink;
using RabbitLink.Consumer;
using RabbitLink.Topology;

namespace ServiceLink.RabbitMq.Configuration
{
    public class NotifyQueueConfig
    {
        public NotifyQueueConfig(string exchangeName, string queueNamePrefix, bool isTemporary, TimeSpan? lifetime,
            ushort prefetchCount, string routingKey, bool noBind, bool autoAck)
        {
            ExchangeName = exchangeName;
            QueueNamePrefix = queueNamePrefix;
            IsTemporary = isTemporary;
            Lifetime = lifetime;
            PrefetchCount = prefetchCount;
            RoutingKey = routingKey;
            NoBind = noBind;
            AutoAck = autoAck;
        }

        public string ExchangeName { get; }
        public string QueueNamePrefix { get; }
        public bool IsTemporary { get; }
        public TimeSpan? Lifetime { get; }
        public ushort PrefetchCount { get; }
        public string RoutingKey { get; }
        public bool NoBind { get; }
        public bool AutoAck { get; }
        
        
        public ILinkConsumer CreateConsumer(Link link)
        {
            return link.CreateConsumer(
                async cfg =>
                {
                    
                    var name = QueueNamePrefix;
                    if (IsTemporary)
                        name = $"{name}.{Guid.NewGuid():D}";
                    ILinkQueue queue;
                    if (!IsTemporary)
                        queue = await cfg.QueueDeclare(name, expires: Lifetime);
                    else
                        queue = await cfg.QueueDeclare(name, expires: Lifetime);
                    if (!NoBind)
                    {
                        var exch = await cfg.ExchangeDeclarePassive(ExchangeName);
                        await cfg.Bind(queue, exch, RoutingKey);
                    }
                    return queue;
                }, config: cfg => cfg.AutoAck(AutoAck).PrefetchCount(PrefetchCount));
        }
        
    }
}