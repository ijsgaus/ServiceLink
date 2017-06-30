using System;
using JetBrains.Annotations;
using ServiceLink.Metadata;

namespace ServiceLink.RabbitMq.Topology
{
    public abstract class ChannelParams
    {
        protected ChannelParams([NotNull] EndPointParams endPoint, [NotNull] ISerializer<byte[]> serializer)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        [NotNull]
        public EndPointParams EndPoint { get; }
        
        [NotNull]
        public ISerializer<byte[]> Serializer { get; }
    }
}