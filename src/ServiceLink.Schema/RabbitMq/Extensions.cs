using ServiceLink.Markers.RabbitMq;

namespace ServiceLink.Schema.RabbitMq
{
    public static class Extensions
    {
        public static string ToJsonString(this ExchangeType exchangeType)
            => exchangeType.ToString().ToLower();
        
        
    }
}