using System;
using System.Collections.Generic;

namespace ServiceLink.Schema.CSharp
{
    public class CSharpCodeGenerationOptions
    {
        public CSharpCodeGenerationOptions(string @namespace)
        {
            Namespace = @namespace;
        }

        public string Namespace { get; }
        public Type DateTimeType { get; set; } = typeof(DateTimeOffset);
        public Type DateType { get; set; } = typeof(DateTimeOffset);
        public Type UnitType { get; set; } = typeof(ValueTuple);
        public string InterfaceName { get; set; }
        public string[] ExcludedTypes { get; set; } = new string[0];
        public IDictionary<string, SkipInclude> GeneratedProperties { get; set; } = new Dictionary<string, SkipInclude>();
        public SkipInclude Endpoints { get; set; } = SkipInclude.IncludeAll; 
    }
}