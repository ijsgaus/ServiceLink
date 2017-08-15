using System.Net.Mime;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ServiceLink.Serialization.Json
{
    public class JsonTextSerialize : ISerialize<string>
    {
        private readonly JsonSerializerSettings _settings;

        public JsonTextSerialize(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public Serialized<string> Serialize(string typeCode, object obj)
        {
            var json = JsonConvert.SerializeObject(obj, _settings);
            var contentType = new ContentType("text/json");
            return new Serialized<string>(typeCode,  contentType, json);
        }
    }
}