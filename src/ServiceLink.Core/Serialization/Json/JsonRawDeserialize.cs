using System.Text;
using LanguageExt;
using Newtonsoft.Json;

namespace ServiceLink.Serialization.Json
{
    public class JsonRawDeserialize : IDeserialize<byte[]>
    {
        private readonly JsonSerializerSettings _settings;

        public JsonRawDeserialize(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public Result<T> Deserialize<T>(Serialized<byte[]> data)
        {
            return Prelude.Try(() => Encoding.UTF8.GetString(data.Data))
                .Bind(p => Prelude.Try(() => JsonConvert.DeserializeObject<T>(p, _settings)))();
        }
    }
}