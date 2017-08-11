using Newtonsoft.Json;

namespace ServiceLink.Schema
{
    public class CallableEndpointSchema : EndpointSchema
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ContractTypeSchema Request { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ContractTypeSchema Response { get; set; }
    }
}