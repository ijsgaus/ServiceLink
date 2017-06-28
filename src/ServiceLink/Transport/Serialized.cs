namespace ServiceLink.Transport
{
    public class Serialized<T> : ISerialized<T>
    {
        public Serialized(ContentType contentType, EncodedType type, T data)
        {
            ContentType = contentType;
            Type = type;
            Data = data;
        }

        public ContentType ContentType { get; }
        public EncodedType Type { get; }
        public T Data { get; }
    }
}