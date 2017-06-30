using Microsoft.Extensions.Logging;

namespace ServiceLink.RabbitMq
{
    public static class LogEvents
    {
        public static readonly EventId PreparePublish = new EventId(2000, "PreparePublish");
        public static readonly EventId Publish = new EventId(2001, "Publish");
    }
}