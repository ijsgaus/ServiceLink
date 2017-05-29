using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        public SerializedMessage Serialize<T>(T data)
        {
            var contract = _contractResolver.GetContract(data);
            if (contract == null) return null;
            return new SerializedMessage(contract, "text/json;encoding=utf8", 
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, _settings)));
        }
    }
}