using System;

namespace ServiceLink.RabbitMq.Markers
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property)]
    public class RoutingKeyAttribute : Attribute
    {
        public RoutingKeyAttribute(string routingKey)
        {
            RoutingKey = routingKey;
        }

        public string RoutingKey { get; }
    }
}