using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ServiceLink.Serialization
{
    public class RawMapper : ISerializedMapper<byte[], string>
    {
        private readonly ILogger<RawMapper> _logger;

        public RawMapper(ILogger<RawMapper> logger)
        {
            _logger = logger;
        }

        public Serialized<string> Map(Serialized<byte[]> serialized)
        {
            if (serialized == null) throw new ArgumentNullException(nameof(serialized));
            var encoding = Encoding.UTF8;
            if (serialized?.ContentType.CharSet != null)
            {
                
                try
                {
                    encoding = Encoding.GetEncoding(serialized.ContentType.CharSet);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(0, ex, "Charset for ContentType {type} unknown, using UTF8", serialized.ContentType);
                }
            }
            else
                _logger.LogWarning("Charset for ContentType {type} not specified, using UTF8", serialized.ContentType);
            return new Serialized<string>(serialized.TypeCode, serialized.ContentType,
                encoding.GetString(serialized.Data));
        }
    }
}