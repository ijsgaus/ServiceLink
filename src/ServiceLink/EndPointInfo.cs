namespace ServiceLink
{
    public class EndPointInfo
    {
        public EndPointInfo(string serviceName, string endpointName)
        {
            ServiceName = serviceName;
            EndpointName = endpointName;
        }

        public string ServiceName { get; }
        public string EndpointName { get; }
    }
}