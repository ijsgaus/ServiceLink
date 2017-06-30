using RabbitLink.Messaging;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Topology
{
    public class PublishParams
    {
        public PublishParams()
            : this(new LinkMessageProperties(), new LinkPublishProperties())
        {
        }

        public PublishParams(LinkMessageProperties messageProperties, LinkPublishProperties publishProperties)
        {
            MessageProperties = messageProperties;
            PublishProperties = publishProperties;
        }

        public LinkMessageProperties MessageProperties { get; }
        public LinkPublishProperties PublishProperties { get; }

        public PublishParams ApplySerialization(ISerialized<byte[]> serialized)
        {
            MessageProperties.ContentType = serialized.ContentType.ToString();
            MessageProperties.Type = serialized.Type.ToString();
            return this;
        }
    }
}