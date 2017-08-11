using Newtonsoft.Json;

namespace ServiceLink.Schema
{
    public abstract class EndpointSchema : SchemaBase
    {
        [JsonProperty(IsReference = true)]
        public ServiceSchema Owner { get; set; }
    }
}