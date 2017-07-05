namespace ServiceLink.Serializers
{
    public class Serialized<TTarget>
    {
        public Serialized(ContentType contentType, EncodedType type, TTarget data)
        {
            ContentType = contentType;
            Type = type;
            Data = data;
        }

        public ContentType ContentType { get; }
        public EncodedType Type { get; }
        public TTarget Data { get; }
    }
}