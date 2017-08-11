using System;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp.Models;
using ServiceLink.Schema.Templates;

namespace ServiceLink.Schema.CSharp
{
    internal class CustomTemplateFactory : ITemplateFactory
    {
        private readonly JsonSchema4 _schema;
        private static readonly ITemplateFactory Default = new DefaultTemplateFactory();

        public CustomTemplateFactory(JsonSchema4 schema)
        {
            _schema = schema;
        }

        public ITemplate CreateTemplate(string package, string template, object model)
        {
            if (model is ClassTemplateModel ctm)
            {
                if (_schema.Definitions.TryGetValue(ctm.Class, out var modelSchema))
                {
                    if (modelSchema.ExtensionData != null && modelSchema.ExtensionData.ContainsKey("X-ContractVersion"))
                    {
                        var options = new CSharpClassTemplateOptions();
                        if (modelSchema.ExtensionData.TryGetValue("X-ContractName", out var contractName))
                            options.ContractName = contractName.ToString();
                        options.ContractVersion =
                            Version.Parse(modelSchema.ExtensionData["X-ContractVersion"].ToString());
                        return new CSharpClassTemplate(ctm, options);
                    }
                    
                }

            }
            return Default.CreateTemplate(package, template, model);
        }
    }
}