using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceLink.Exceptions;
using ServiceLink.Monads;

namespace ServiceLink.Bus
{
    public class JsonBusSerializer : IBusSerializer
    {
        private readonly JsonSerializerSettings _settings;
        private readonly IBusContractResolver _contractResolver;

        public JsonBusSerializer(IBusContractResolver contractResolver)
            : this(contractResolver, true)
        {
            
        }

        public JsonBusSerializer(IBusContractResolver contractResolver, bool camelCase)
            : this(contractResolver, new JsonSerializerSettings
            {
                ContractResolver = camelCase ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver()
            })
        {
            
        }

        public JsonBusSerializer(IBusContractResolver contractResolver, JsonSerializerSettings settings)
        {
            _settings = settings;
            _contractResolver = contractResolver;
        }

        public Result<SerializedMessage> Serialize<T>(T data)
        {
            var type = data?.GetType() ?? typeof(T);
            var contract = _contractResolver.PrepareContract(type, data);
            if (contract == null) return new MissedContractException($"Contract for {type} {data} not found").ToError<SerializedMessage>();
            return new SerializedMessage(contract.Code, "text/json;encoding=utf8", 
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(contract.Body, contract.BodyType, _settings))).ToSuccess();
        }
    }
}