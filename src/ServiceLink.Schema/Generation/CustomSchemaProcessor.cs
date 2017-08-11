using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NJsonSchema.Generation;
using ServiceLink.Markers;

namespace ServiceLink.Schema.Generation
{
    internal class CustomSchemaProcessor : ISchemaProcessor
    {
        public Task ProcessAsync(SchemaProcessorContext context)
        {
            var contractAttr = context.Type.GetTypeInfo().GetCustomAttribute<ContractAttribute>();
            if(contractAttr == null) return Task.CompletedTask;
            context.Schema.ExtensionData = new Dictionary<string, object>();
            if(contractAttr.Name != null)
                context.Schema.ExtensionData["X-ContractName"] = new JValue(contractAttr.Name);
            context.Schema.ExtensionData["X-ContractVersion"] = new JValue(contractAttr.Version.ToString());
            return Task.CompletedTask;
        }
    }
}