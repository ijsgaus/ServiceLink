using System;
using System.Reflection;
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
            var (name, type) = FindExchange(serviceTypeInfo, endpoint.Info.Member, endpoint.ServiceName, LinkExchangeType.Direct);
            var routingKey = FindRoutingKey(serviceTypeInfo, endpoint.Info.Member, endpoint.EndpointName);
            
            var confirmMode = true;
            TimeSpan? messageTtl = null;
            
            
            var confirmModeAttr = serviceTypeInfo.GetCustomAttribute<ConfirmModeAttribute>();
            if (confirmModeAttr != null)
            {
                confirmMode = confirmModeAttr.ConfirmMode;
            }
            
            var msgTtlAttr = serviceTypeInfo.GetCustomAttribute<MessageTtlAttribute>();
            if (msgTtlAttr != null)
            {
                messageTtl = msgTtlAttr.MessageTtl;
            }
            
            
            confirmModeAttr = endpoint.Info.Member.GetCustomAttribute<ConfirmModeAttribute>();
            if (confirmModeAttr != null)
            {
                confirmMode = confirmModeAttr.ConfirmMode;
            }
            
            msgTtlAttr = endpoint.Info.Member.GetCustomAttribute<MessageTtlAttribute>();
            if (msgTtlAttr != null)
            {
                messageTtl = msgTtlAttr.MessageTtl;
            }
            return new NotifyExchangeConfig(name, confirmMode, type, messageTtl, routingKey);
        }
        
        

        public Func<Link, ILinkConsumer> GetNotifyQueueFactory(NotifyEndpoint endpoint)
        {
            var typeInfo = endpoint.Info.ServiceType.GetTypeInfo();
            var (exchangeName, _) = FindExchange(typeInfo, endpoint.Info.Member, endpoint.ServiceName, LinkExchangeType.Direct);
            
        }

        private (string, LinkExchangeType) FindExchange(TypeInfo service, MemberInfo member, string defaultName, LinkExchangeType defaultType)
        {
            var exchangeAttr = service.GetCustomAttribute<ExchangeAttribute>();
            if (exchangeAttr != null)
            {
                if (exchangeAttr.Name != null)
                    defaultName = exchangeAttr.Name;
                switch (exchangeAttr.Type)
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
                
            }
            
            exchangeAttr = member.GetCustomAttribute<ExchangeAttribute>();
            if (exchangeAttr != null)
            {
                if (exchangeAttr.Name != null)
                    defaultName = exchangeAttr.Name;
                switch (exchangeAttr.Type)
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
                
            }

            return (defaultName, defaultType);
        }

        private string FindRoutingKey(TypeInfo service, MemberInfo member, string @default)
        {
            var routingKeyAttr = service.GetCustomAttribute<RoutingKeyAttribute>();
            if (routingKeyAttr != null)
            {
                @default = routingKeyAttr.RoutingKey;
            }
            
            routingKeyAttr = member.GetCustomAttribute<RoutingKeyAttribute>();
            if (routingKeyAttr != null)
            {
                @default = routingKeyAttr.RoutingKey;
            }
            return @default;
        }
    }
}