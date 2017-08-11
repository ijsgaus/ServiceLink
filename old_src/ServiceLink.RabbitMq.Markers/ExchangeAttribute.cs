using System;

namespace ServiceLink.RabbitMq.Markers
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property)]
    public class ExchangeAttribute : Attribute
    {
        public ExchangeAttribute(ExchangeType type) : this(null, type)
        {
        }

        public ExchangeAttribute(string name, ExchangeType type = ExchangeType.Direct)
        {
            Name = name;
            Type = type;
        }
        
        

        public string Name { get; }
        public ExchangeType Type { get; }
    }
}