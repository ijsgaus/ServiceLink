using System;
using Newtonsoft.Json;

namespace ServiceLink.Exceptions
{
    [Serializable]
    [JsonConverter(typeof(JsonSerializedExceptionJsonConverter))]
    public abstract class SerializedException : Exception
    {
        protected SerializedException(Exception ex)
            : base(ex.Message)
        {
            OriginalType = ex.GetType().FullName;
        }

        protected SerializedException(string message) : base(message)
        {
        }

        public string OriginalType { get; protected set; }
        public string OriginalInfo { get; protected set; }
    }
}