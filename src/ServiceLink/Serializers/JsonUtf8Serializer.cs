using System.Text;
using ServiceLink.Transport;
using Newtonsoft.Json;

namespace ServiceLink.Serializers
{
    public class JsonUtf8Serializer : ISerializer<byte[]>
    {
        private readonly JsonSerializerSettings _settings;

        public JsonUtf8Serializer(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? new JsonSerializerSettings();
        }

        public ISerialized<byte[]> Serialize<TMessage>(TMessage message)
        {
            return new Serialized<byte[]>(new ContentType("text/json;encoding=utf8"), new EncodedType(typeof(TMessage).FullName),  
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, _settings)));
        }

        public Result<TMessage> TryDeserialize<TMessage>(ISerialized<byte[]> serialized)
        {
            return Result.Try(() => JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(serialized.Data)));
        }
    }
}