using System;
using System.Threading.Tasks;

namespace ServiceLink.Bus
{
    public interface ISafePublishSerializer
    {
        SerializedMessage Serialize<T>(T data);
        
    }

    public interface ISafePublishDeserializer
    {
        (Type, object) Deserialize(SerializedMessage message);
    }

    public class SerializedMessage
    {
        public SerializedMessage(string type, string contentType, byte[] data)
        {
            Type = type;
            ContentType = contentType;
            Data = data;
        }

        public string Type { get; }
        public string ContentType { get; }
        public byte[] Data { get; }
    }
}