using System;
using Newtonsoft.Json;

namespace ServiceLink.Exceptions
{
    [Serializable]
    public class JsonSerializedException : SerializedException
    {
        public JsonSerializedException(Exception ex) : base(ex)
        {
            OriginalInfo = JsonConvert.SerializeObject(ex,
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        }

        internal JsonSerializedException(string message, string originalType, string originalInfo)
            : base(message)
        {
            OriginalInfo = originalInfo;
            OriginalType = originalType;
        }

        public static JsonSerializedException Serialize(Exception ex)
        {
            return ex is JsonSerializedException jse ? jse : new JsonSerializedException(ex);
        }
    }
}