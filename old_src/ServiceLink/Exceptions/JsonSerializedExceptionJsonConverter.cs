using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ServiceLink.Exceptions
{
    public class JsonSerializedExceptionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(SerializedException).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var ex = (SerializedException) value;
            var payload = new Payload
            {
                Message = ex.Message,
                OriginalType = ex.OriginalType,
                OriginalInfo = ex.OriginalInfo
            };
            serializer.Serialize(writer, payload);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var payload = serializer.Deserialize<Payload>(reader);
            return new JsonSerializedException(payload.Message, payload.OriginalType, payload.OriginalInfo);
        }

        internal class Payload
        {
            public string Message { get; set; }
            public string OriginalType { get; set; }
            public string OriginalInfo { get; set; }
        }
    }
}