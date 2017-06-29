using RabbitLink.Messaging;

namespace ServiceLink.RabbitMq
{
    internal class ProduceParams
    {
        public ProduceParams()
            : this(new LinkMessageProperties(), new LinkPublishProperties())
        {
        }

        public ProduceParams(LinkMessageProperties messageProperties, LinkPublishProperties publishProperties)
        {
            MessageProperties = messageProperties;
            PublishProperties = publishProperties;
        }

        public LinkMessageProperties MessageProperties { get; }
        public LinkPublishProperties PublishProperties { get; }
    }
}