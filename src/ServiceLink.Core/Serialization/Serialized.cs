namespace ServiceLink.Serialization
{
    public class Serialized<TFormat>
    {
        public Serialized(string typeCode, string encoding, TFormat data)
        {
            TypeCode = typeCode;
            Encoding = encoding;
            Data = data;
        }

        public string TypeCode { get; }
        public string Encoding { get; }
        public TFormat Data { get; }
    }
}