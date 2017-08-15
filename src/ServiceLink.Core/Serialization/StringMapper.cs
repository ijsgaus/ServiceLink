using System.Net.Mime;
using System.Text;

namespace ServiceLink.Serialization
{
    public class StringMapper : ISerializedMapper<string, byte[]>
    {
        private readonly Encoding _encoding;

        public StringMapper(Encoding encoding)
        {
            _encoding = encoding;
        }

        public Serialized<byte[]> Map(Serialized<string> serialized)
        {
            var contentType = new ContentType(serialized.ContentType.ToString()) {CharSet = _encoding.WebName};
            return new Serialized<byte[]>(serialized.TypeCode, contentType,
                _encoding.GetBytes(serialized.Data));
        }

        public static readonly ISerializedMapper<string, byte[]> Utf8 = new StringMapper(Encoding.UTF8);
    }
}