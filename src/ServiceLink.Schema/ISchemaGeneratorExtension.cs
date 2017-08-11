using System;
using System.Reflection;

namespace ServiceLink.Schema
{
    public interface ISchemaGeneratorExtension
    {
        void ExtendService(Type serviceType, ServiceSchema schema);
        void ExtendEndpoint(PropertyInfo propertyInfo, EndpointSchema schema);
    }
}