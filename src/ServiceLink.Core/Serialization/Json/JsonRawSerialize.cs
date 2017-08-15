using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace ServiceLink.Serialization.Json
{
    public class JsonRawSerialize : ISerialize<byte[]>
    {
        private readonly JsonSerializerSettings _settings;

        public JsonRawSerialize(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public Serialized<byte[]> Serialize(string typeCode, object obj)
        {
            var text = JsonConvert.SerializeObject(obj, _settings);
            var bytes = Encoding.UTF8.GetBytes(text);
            var contentType = new ContentType("text/json") { CharSet = Encoding.UTF8.WebName };
            return new Serialized<byte[]>(typeCode, contentType, bytes);
        }
    }
}