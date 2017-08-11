using Newtonsoft.Json;

namespace ServiceLink.Schema
{
    public class CommandEndpointSchema : EndpointSchema
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ContractTypeSchema Command { get; set; }
    }
}