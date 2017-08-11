using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ServiceLink.Schema
{
    public class ServiceSchema : SchemaBase
    {
        public string Name { get; set; }
        public Version Version { get; set; }


        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        public Dictionary<string, EndpointSchema> Endpoints { get; set; } = new Dictionary<string, EndpointSchema>();


        public JObject Contracts { get; set; }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool pretty)
        {
            return ToJson().ToString(pretty ? Formatting.Indented : Formatting.None);
        }

        public JObject ToJson()
        {
            return JObject.FromObject(this, JsonSerializer.Create(SerializerSettings));
        }

        public void ToFile(string fileName, bool pretty = true)
        {
            File.WriteAllText(fileName, ToString(pretty));
        }


        

        public static ServiceSchema FromString(string jsonString)
        {
            return FromJson(JObject.Parse(jsonString));
        }

        public static ServiceSchema FromFile(string fileName)
        {
            return FromString(File.ReadAllText(fileName));
        }

        public static ServiceSchema FromJson(JObject json)
        {
            return json.ToObject<ServiceSchema>(JsonSerializer.Create(SerializerSettings));
        }

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            SerializationBinder = new ServiceSchemaSerializationBinder(),
            Converters = new JsonConverter[] { new VersionConverter(), },
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize

        };

    }
}