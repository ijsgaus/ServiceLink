using System;
using System.Text;
using Newtonsoft.Json;
using ServiceLink.Exceptions;
using ServiceLink.Monads;

namespace ServiceLink.Bus
{
    public class JsonBusDeserializer : IBusDeserializer
    {
        private readonly IBusContractResolver _resolver;
        private readonly JsonSerializerSettings _settings;

        public JsonBusDeserializer(IBusContractResolver resolver)
            : this(resolver, new JsonSerializerSettings())
        {
            
        }

        public JsonBusDeserializer(IBusContractResolver resolver, JsonSerializerSettings settings)
        {
            _resolver = resolver;
            _settings = settings;
        }

        public Result<(Type, object)> Deserialize(SerializedMessage message)
        {
            if (message.ContentType == "text/json;encoding=utf8")
            {
                var extractor = _resolver.GetExtractor(message.Type);
                if (extractor == null)
                    return new MissedContractException($"Contract extractor for {message.Type} not found").ToError<(Type, object)>();
                try
                {
                    var jsonText = Encoding.UTF8.GetString(message.Data);
                    var obj = JsonConvert.DeserializeObject(jsonText, extractor.BodyType, _settings);
                    var result = extractor.BodyToResult(obj);
                    return (extractor.ResultType, result).ToSuccess();
                }
                catch (Exception ex)
                {
                    return ex.ToError<(Type, object)>();
                }

            }
            return ((Exception) null).ToError<(Type, object)>();
        }
    }
}