using Newtonsoft.Json;

namespace ServiceLink.Schema
{
    public class EventEndpointSchema : EndpointSchema
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ContractTypeSchema Event { get; set; }
    }
}