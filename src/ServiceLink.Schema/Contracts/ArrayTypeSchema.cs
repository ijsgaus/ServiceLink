using Newtonsoft.Json;

namespace ServiceLink.Schema
{
    public class ArrayTypeSchema : ContractTypeSchema
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ContractTypeSchema Element { get; set; } 
    }
}