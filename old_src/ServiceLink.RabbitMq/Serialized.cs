namespace ServiceLink.RabbitMq
{
    internal class Serialized<TMessage> : ISerilized<byte[]>
    {
        public Serialized(ContentType contentType, EncodedType type, byte[] data)
        {
            ContentType = contentType;
            Type = type;
            Data = data;
        }

        public ContentType ContentType { get; }
        public EncodedType Type { get; }
        public byte[] Data { get; }
    }
}