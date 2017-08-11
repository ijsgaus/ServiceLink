using System;
using System.Reflection;
using ServiceLink.Markers.RabbitMq;

namespace ServiceLink.Schema.RabbitMq
{
    public class RabbitSchemaGenerator : ISchemaGeneratorExtension
    {
        public void ExtendService(Type serviceType, ServiceSchema schema)
        {
            var typeInfo = serviceType.GetTypeInfo();
            var serviceExt = new ServiceSchemaExtension(schema);
            ProcessCommonAttributes(typeInfo, serviceExt);
            
        }

        public void ExtendEndpoint(PropertyInfo propertyInfo, EndpointSchema schema)
        {
            var endpointExt = new EndpointSchemaExtension(schema);
            ProcessCommonAttributes(propertyInfo, endpointExt);
        }
        
        private static void ProcessCommonAttributes(MemberInfo typeInfo, SchemaExtension schema)
        {
            var inExchangeAttr = typeInfo.GetCustomAttribute<ExchangeInAttribute>();
            if (inExchangeAttr != null)
            {
                if (inExchangeAttr.Name != null)
                    schema.ExchangeIn = inExchangeAttr.Name;
                schema.ExchangeInType = inExchangeAttr.Type;
            }

            var outExchangeAttr = typeInfo.GetCustomAttribute<ExchangeOutAttribute>();
            if (outExchangeAttr != null)
            {
                if (outExchangeAttr.Name != null)
                    schema.ExchangeIn = outExchangeAttr.Name;
                schema.ExchangeInType = outExchangeAttr.Type;
            }
            var rkAttr = typeInfo.GetCustomAttribute<RoutingKeyAttribute>();
            if (rkAttr != null)
                schema.RoutingKey = rkAttr.Key;
        }

        
    }
}