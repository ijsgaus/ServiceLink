using System.Net;
using ServiceLink.Metadata;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Topology
{
    public class NotifyPublishConfigure : IPublishConfigure
    {
        public PublishParams Configure<TMessage>(TMessage message, ProducerParams @params, SendParams sendParams)
        {
            if (@params.EndPoint.Type != EndPointType.Notify) return null;
            var publishParams = new PublishParams();
            publishParams.MessageProperties.AppId = @params.EndPoint.HolderName;
            publishParams.PublishProperties.RoutingKey = @params.RoutingKey;
            return publishParams;
        }
    }
}