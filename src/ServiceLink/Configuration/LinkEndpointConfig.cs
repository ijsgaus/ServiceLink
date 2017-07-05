using JetBrains.Annotations;

namespace ServiceLink.Configuration
{
    public class LinkEndpointConfig
    {
        public LinkEndpointConfig([NotNull] string serviceName, [NotNull] string endpointName)
        {
            ServiceName = serviceName;
            EndpointName = endpointName;
        }

        [NotNull]
        public string ServiceName { get; }
        [NotNull]
        public string EndpointName { get; }
    }
}