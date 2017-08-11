using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServiceLink.Schema
{
    public class SchemaBase
    {

        public string Title { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();

        public bool ShouldSerializeAdditionalData()
        {
            return AdditionalData.Count > 0;
        }
    }
}