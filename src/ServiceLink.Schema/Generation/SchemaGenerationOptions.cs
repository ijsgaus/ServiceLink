using System;

namespace ServiceLink.Schema.Generation
{
    public class SchemaGenerationOptions
    {
        public SchemaGenerationOptions()
        {
        }

        public SchemaGenerationOptions(params ISchemaGeneratorExtension[] extensions)
        {
            Extensions = extensions;
        }

        public Func<string, bool, string> MemberNameToSchemaName { get; set; } = null;
        public Type UnitType { get; set; } = typeof(ValueTuple);
        public Type[] OtherUnitTypes { get; set; } = new Type[0];
        public ISchemaGeneratorExtension[] Extensions { get; set; } = new ISchemaGeneratorExtension[0];
    }
}