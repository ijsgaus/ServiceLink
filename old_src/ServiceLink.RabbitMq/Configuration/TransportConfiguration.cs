using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using RabbitLink;
using RabbitLink.Consumer;
using RabbitLink.Topology;
using ServiceLink.Endpoints;
using ServiceLink.RabbitMq.Markers;
using ServiceLink.RabbitMq.Topology;

namespace ServiceLink.RabbitMq.Configuration
{
    public class TransportConfiguration : ITransportConfiguration
    {
        public IExchangeConfig GetNotifyExchangeConfig(NotifyEndpoint endpoint)
        {
            var serviceTypeInfo = endpoint.Info.ServiceType.GetTypeInfo();
            var (name, type) = FindAttribute<ExchangeAttribute, (string, LinkExchangeType)>((endpoint.ServiceName, LinkExchangeType.Direct), 
                a => (a.Name, ToLinkExchangeType(a.Type) ), a => true, serviceTypeInfo, endpoint.Info.Member );
            
            var routingKey = FindAttribute<RoutingKeyAttribute, string>(endpoint.EndpointName, a => a.RoutingKey, a => true,  serviceTypeInfo, endpoint.Info.Member);
            
            var confirmMode = FindAttribute<ConfirmModeAttribute, bool>(true, p => p.ConfirmMode, p => true, serviceTypeInfo, endpoint.Info.Member );
            var messageTtl = FindAttribute<MessageTtlAttribute, TimeSpan?>(null, p => p.MessageTtl, p => true, serviceTypeInfo, endpoint.Info.Member);
            
                        
            return new NotifyExchangeConfig(name, confirmMode, type, messageTtl, routingKey);
        }
        
        

        public Func<Link, ILinkConsumer> GetNotifyQueueFactory(NotifyEndpoint endpoint)
        {
            var typeInfo = endpoint.Info.ServiceType.GetTypeInfo();
            var exchangeName = FindAttribute<ExchangeAttribute, string>(endpoint.ServiceName, a => a.Name, a => true, typeInfo,   endpoint.Info.Member);
            var queueFormat = FindAttribute<QueueNameAttribute, string>( "{exchange}.{holder}", a => a.Name, a => true, endpoint.Info.Member);
            queueFormat = queueFormat.Replace("{exchange}", exchangeName);
            queueFormat = queueFormat.Replace("{holder}", endpoint.Holder);
            if (endpoint.SubscribeName != null)
                queueFormat = $"{queueFormat}.{endpoint.SubscribeName}";
            var routingKey = FindAttribute<RoutingKeyAttribute, string>(endpoint.EndpointName, a => a.RoutingKey, a => true,  typeInfo, endpoint.Info.Member);;
            TimeSpan? expires;
            bool isTemporary;
            if (endpoint.ObserveKind == NotifyObserveKind.PerName)
            {
                isTemporary = false;
                expires = FindAttribute<SessionQueueExpiresAttribute, TimeSpan?>(null, a => a.Lifetime, a => true,
                    endpoint.Info.Member);

            }
            else
            {
                isTemporary = true;
                expires = FindAttribute<QueueExpiresAttribute, TimeSpan?>(null, a => a.Lifetime, a => true,
                    endpoint.Info.Member);
            }
            return new NotifyQueueConfig(exchangeName, queueFormat, isTemporary, expires, endpoint.PrefetchCount, routingKey, false, false).CreateConsumer;
        }


        private static TValue FindAttribute<TAttr, TValue>(TValue @default, Func<TAttr, TValue> extractor,
            Func<TAttr, bool> predicate, params MemberInfo[] where) where TAttr : Attribute
        {
            foreach (var info in where)
            {
                foreach (var attr in info.GetCustomAttributes<TAttr>().Where(predicate))
                {
                    @default = extractor(attr);
                }
                
            }
            return @default;
        }
        
        
        
        
        
        

        private static LinkExchangeType ToLinkExchangeType(ExchangeType exchangeAttrType)
        {
            LinkExchangeType defaultType;
            switch (exchangeAttrType)
            {
                case ExchangeType.Fanout:
                    defaultType = LinkExchangeType.Fanout;
                    break;
                case ExchangeType.Direct:
                    defaultType = LinkExchangeType.Direct;
                    break;
                case ExchangeType.Topic:
                    defaultType = LinkExchangeType.Topic;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return defaultType;
        }

        
    }
}