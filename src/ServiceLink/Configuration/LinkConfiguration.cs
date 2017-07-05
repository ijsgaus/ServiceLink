using ServiceLink.Endpoints;

namespace ServiceLink.Configuration
{
    public delegate LinkEndpointConfig LinkConfiguration(EndpointInfoBase endpoint, IHolder holder, LinkConfigure config);
}