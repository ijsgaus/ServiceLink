using System;

namespace ServiceLink.RabbitMq.Markers
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Property)]
    public class MessageTtlAttribute : Attribute
    {
        public MessageTtlAttribute(string messageTtl)
        {
            if (messageTtl == null)
                MessageTtl = null;
            else
                MessageTtl = TimeSpan.Parse(messageTtl);
        }

        public TimeSpan? MessageTtl { get; }
    }
}