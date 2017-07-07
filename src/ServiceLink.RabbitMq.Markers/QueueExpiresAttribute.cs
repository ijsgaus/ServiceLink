using System;

namespace ServiceLink.RabbitMq.Markers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueueExpiresAttribute : Attribute
    {
        public QueueExpiresAttribute(string period)
        {
            if (period == null)
                Lifetime = null;
            else
            {
                Lifetime = TimeSpan.Parse(period);
            }
        }

        public TimeSpan? Lifetime { get; }
    }
}