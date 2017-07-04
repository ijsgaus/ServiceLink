using System.Reflection;

namespace ServiceLink.Metadata
{
    public class EndPointParams
    {
        public EndPointParams(EndPointType type, IHolder holder, string serviceName, string endpointName,
            MemberInfo contract, ISerializer<byte[]> serializer)
        {
            Type = type;
            Holder = holder;
            ServiceName = serviceName;
            EndpointName = endpointName;
            Contract = contract;
            Serializer = serializer;
        }

        public EndPointType Type { get; }
        public IHolder Holder { get; }
        public string ServiceName { get; }
        public string EndpointName { get; }
        public MemberInfo Contract { get; }
        public ISerializer<byte[]> Serializer { get; }
    }
}