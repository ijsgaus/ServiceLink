using System;
using Newtonsoft.Json.Serialization;

namespace ServiceLink.Schema
{
    public class ServiceSchemaSerializationBinder : ISerializationBinder
    {
        public Type BindToType(string assemblyName, string typeName)
        {
            switch (typeName)
            {
                case "event":
                    return typeof(EventEndpointSchema);
                case "command":
                    return typeof(CommandEndpointSchema);
                case "callable":
                    return typeof(CallableEndpointSchema);
                case "object":
                    return typeof(ObjectTypeSchema);
                case "wellKnown":
                    return typeof(WellKnownTypeSchema);
                case "array":
                    return typeof(ArrayTypeSchema);
                default:
                    throw new InvalidOperationException($"Unknow type {assemblyName} {typeName}");
            }
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            if (serializedType == typeof(EventEndpointSchema))
            {
                typeName = "event";
                return;
            }
            if (serializedType == typeof(CommandEndpointSchema))
            {
                typeName = "command";
                return;
            }
            if (serializedType == typeof(CallableEndpointSchema))
            {
                typeName = "callable";
                return;
            }
            if (serializedType == typeof(ObjectTypeSchema))
            {
                typeName = "object";
                return;
            }
            if (serializedType == typeof(WellKnownTypeSchema))
            {
                typeName = "wellKnown";
                return;
            }
            if (serializedType == typeof(ArrayTypeSchema))
            {
                typeName = "array";
                return;
            }
            throw new InvalidOperationException($"Unknow type {serializedType}");
        }
    }
}