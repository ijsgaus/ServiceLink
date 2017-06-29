using System.Reflection;

namespace ServiceLink.Metadata
{
    public class EndPointParams
    {
        public string HolderName { get; }
        public string ServiceName { get; }
        public string EndpointName { get; }
        public MemberInfo Contract { get; }
        public ISerializer<byte[]> Serializer { get; }
    }
}