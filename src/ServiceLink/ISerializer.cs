using Newtonsoft.Json;

namespace ServiceLink
{
    public interface ISerializer<TTarget>
    {
        ISerialized<TTarget> Serialize<TMessage>(TMessage message);
        Result<TMessage> TryDeserialize<TMessage>(ISerialized<TTarget> serialized);
    }

    public interface ISerialized<out TTarget>
    {
        ContentType ContentType { get; }
        EncodedType Type { get; }
        TTarget Data { get; }
    }

    public class EncodedType
    {
        private readonly string _encodedType;

        internal EncodedType(string encodedType)
        {
            _encodedType = encodedType;
        }

        public override string ToString()
        {
            return _encodedType;
        }
        
        public static EncodedType Parse(string encodedType)
            => new EncodedType(encodedType);
    }

    public class ContentType
    {
        private readonly string _contentType;

        internal ContentType(string contentType)
        {
            _contentType = contentType;
        }

        public override string ToString()
        {
            return _contentType;
        }
        
        public static ContentType Parse(string contentType)
            => new ContentType(contentType);
    }

    
}