using System;

namespace ServiceLink.RabbitMq.Markers
{
    /// <summary>
    /// Queue name
    /// Known placeholders:
    /// {holder} - holder name
    /// {exchange} - exchange name
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class QueueNameAttribute : Attribute
    {
        public QueueNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}