namespace ServiceLink.Bus
{
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