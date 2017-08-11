using System.Linq;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using ServiceLink.Markers.RabbitMq;

namespace ServiceLink.Schema.RabbitMq
{

    internal abstract class SchemaExtension
    {
        private readonly SchemaBase _schema;
        private readonly JsonSerializer _serializer;

        protected SchemaExtension(SchemaBase schema, JsonSerializerSettings settings = null)
        {
            _schema = schema;
            settings = settings ?? new JsonSerializerSettings
            {
                Converters = new JsonConverter[] { new StringEnumConverter() } 
            };
            _serializer = JsonSerializer.Create(settings);
        }

        public Option<string> ExchangeIn
        {
            get => ReadValue<string>(nameof(ExchangeIn).ToCamelCase());
            set => WriteValue(nameof(ExchangeIn).ToCamelCase(), value);
        }

        public Option<ExchangeType> ExchangeInType
        {
            get => ReadValue<ExchangeType>(nameof(ExchangeInType).ToCamelCase());
            set => WriteValue(nameof(ExchangeInType).ToCamelCase(), value);
        }

        public Option<string> ExchangeOut
        {
            get => ReadValue<string>(nameof(ExchangeOut).ToCamelCase());
            set => WriteValue(nameof(ExchangeOut).ToCamelCase(), value);
        }

        public Option<ExchangeType> ExchangeOutType
        {
            get => ReadValue<ExchangeType>(nameof(ExchangeOutType).ToCamelCase());
            set => WriteValue(nameof(ExchangeOutType).ToCamelCase(), value);
        }

        public Option<string> RoutingKey
        {
            get => ReadValue<string>(nameof(RoutingKey).ToCamelCase());
            set => WriteValue(nameof(RoutingKey).ToCamelCase(), value);
        }

        protected Option<TValue> ReadValue<TValue>(string propertyName)
        {
            return GetSection()
                .Bind(p => p.TryGetValue(propertyName))
                .Map(p => p.ToObject<TValue>(_serializer));
        }

        private Option<JObject> GetSection()
            => _schema.AdditionalData.TryGetValue(SchemaNames.RabbitMqSection).Map(p => (JObject) p);

        protected void WriteValue<TValue>(string propertyName, TValue value)
        {
            GetSection().IfNone(() =>
            {
                var section = new JObject();
                _schema.AdditionalData.Add(SchemaNames.RabbitMqSection, section);
                return section;
            })[propertyName] = JToken.FromObject(value, _serializer);
        }
        
        protected void WriteValue<TValue>(string propertyName, Option<TValue> value)
        {
            value.Match(p  =>
            GetSection().IfNone(() =>
            {
                var section = new JObject();
                _schema.AdditionalData.Add(SchemaNames.RabbitMqSection, section);
                return section;
            })[propertyName] = JToken.FromObject(p, _serializer), () => GetSection().IfSome(_ => RemoveValue(propertyName)));
        }

        protected void RemoveValue(string propertyName)
        {
            GetSection().IfSome(p =>
            {
                p.Remove(propertyName);
                if (!p.Properties().Any())
                    _schema.AdditionalData.Remove(SchemaNames.RabbitMqSection);
            });
        }
    }
    
    internal abstract class SchemaExtension<T> : SchemaExtension
        where T : SchemaBase
    {


        public T Schema { get; }


        protected SchemaExtension(T schema) : base(schema)
        {
            Schema = schema;
        }
    }
}