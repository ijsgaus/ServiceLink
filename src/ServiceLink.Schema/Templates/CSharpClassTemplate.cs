﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ServiceLink.Schema.Templates
{
    using NJsonSchema;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    internal partial class CSharpClassTemplate : CSharpClassTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(" \r\n");
            
            #line 3 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(Model.HasDescription){
            
            #line default
            #line hidden
            this.Write("/// <summary>");
            
            #line 4 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ConversionUtilities.ConvertCSharpDocBreaks(Model.Description, 0)));
            
            #line default
            #line hidden
            this.Write("</summary>\r\n");
            
            #line 5 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 6 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(Model.HasDiscriminator){
            
            #line default
            #line hidden
            this.Write("[Newtonsoft.Json.JsonConverter(typeof(JsonInheritanceConverter), \"");
            
            #line 7 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Discriminator));
            
            #line default
            #line hidden
            this.Write("\")]\r\n");
            
            #line 8 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  foreach (var derivedClass in Model.DerivedClasses){
            
            #line default
            #line hidden
            this.Write("[JsonInheritanceAttribute(\"");
            
            #line 9 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(derivedClass.Key));
            
            #line default
            #line hidden
            this.Write("\", typeof(");
            
            #line 9 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(derivedClass.Value));
            
            #line default
            #line hidden
            this.Write("))]\r\n");
            
            #line 10 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  }
            
            #line default
            #line hidden
            
            #line 11 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 12 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(Model.DerivedClasses != null){
    foreach(var derivedClass in Model.DerivedClasses) {
            
            #line default
            #line hidden
            this.Write("[System.Runtime.Serialization.KnownType(typeof(");
            
            #line 14 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(derivedClass.Value));
            
            #line default
            #line hidden
            this.Write("))]\r\n");
            
            #line 15 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}}
            
            #line default
            #line hidden
            this.Write("[System.CodeDom.Compiler.GeneratedCode(\"NJsonSchema\", \"");
            
            #line 16 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(JsonSchema4.ToolchainVersion));
            
            #line default
            #line hidden
            this.Write("\")]\r\n");
            
            #line 17 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(Options != null) {
            
            #line default
            #line hidden
            this.Write("[ServiceLink.Markers.Contract(\"");
            
            #line 18 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Options.ContractVersion.ToString()));
            
            #line default
            #line hidden
            this.Write("\"");
            
            #line 18 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(Options.ContractName != null) {
            
            #line default
            #line hidden
            this.Write(", \"");
            
            #line 18 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Options.ContractName));
            
            #line default
            #line hidden
            this.Write("\"");
            
            #line 18 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write(")]\t\r\n");
            
            #line 19 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("public partial class ");
            
            #line 20 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Class));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 20 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Inheritance));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n");
            
            #line 22 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(Model.Inpc){
            
            #line default
            #line hidden
            
            #line 23 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
foreach(var property in Model.Properties){
            
            #line default
            #line hidden
            this.Write("    private ");
            
            #line 24 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Type));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 24 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.FieldName));
            
            #line default
            #line hidden
            
            #line 24 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(property.HasDefaultValue){
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 24 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.DefaultValue));
            
            #line default
            #line hidden
            
            #line 24 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 25 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 27 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 28 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
foreach(var property in Model.Properties){
            
            #line default
            #line hidden
            
            #line 29 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  if(property.HasDescription){
            
            #line default
            #line hidden
            this.Write("    /// <summary>");
            
            #line 30 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ConversionUtilities.ConvertCSharpDocBreaks(property.Description, 1)));
            
            #line default
            #line hidden
            this.Write("</summary>\r\n");
            
            #line 31 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  }
            
            #line default
            #line hidden
            this.Write("    [Newtonsoft.Json.JsonProperty(\"");
            
            #line 32 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write("\", Required = ");
            
            #line 32 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.JsonPropertyRequired));
            
            #line default
            #line hidden
            
            #line 32 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(property.IsStringEnumArray){
            
            #line default
            #line hidden
            this.Write(", ItemConverterType = typeof(Newtonsoft.Json.Converters.StringEnumConverter)");
            
            #line 32 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write(")]\r\n");
            
            #line 33 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  if(property.RenderRequiredAttribute){
            
            #line default
            #line hidden
            this.Write("    [System.ComponentModel.DataAnnotations.Required]\r\n");
            
            #line 35 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  }
            
            #line default
            #line hidden
            
            #line 36 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  if(property.RenderRangeAttribute){
            
            #line default
            #line hidden
            this.Write("    [System.ComponentModel.DataAnnotations.Range(");
            
            #line 37 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.RangeMinimumValue));
            
            #line default
            #line hidden
            this.Write(", ");
            
            #line 37 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.RangeMaximumValue));
            
            #line default
            #line hidden
            this.Write(")]\r\n");
            
            #line 38 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  }
            
            #line default
            #line hidden
            
            #line 39 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  if(property.RenderStringLengthAttribute){
            
            #line default
            #line hidden
            this.Write("    [System.ComponentModel.DataAnnotations.StringLength(");
            
            #line 40 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.StringLengthMaximumValue));
            
            #line default
            #line hidden
            
            #line 40 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  if(property.StringLengthMinimumValue > 0){ 
            
            #line default
            #line hidden
            this.Write(", MinimumLength = ");
            
            #line 40 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.StringLengthMinimumValue));
            
            #line default
            #line hidden
            
            #line 40 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write(")]\r\n");
            
            #line 41 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  }
            
            #line default
            #line hidden
            
            #line 42 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  if(property.RenderRegularExpressionAttribute){
            
            #line default
            #line hidden
            this.Write("    [System.ComponentModel.DataAnnotations.RegularExpression(@\"");
            
            #line 43 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.RegularExpressionValue));
            
            #line default
            #line hidden
            this.Write("\")]\r\n");
            
            #line 44 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  }
            
            #line default
            #line hidden
            
            #line 45 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(property.IsStringEnum){
            
            #line default
            #line hidden
            this.Write("    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumCo" +
                    "nverter))]\r\n");
            
            #line 47 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  }
            
            #line default
            #line hidden
            this.Write("    public ");
            
            #line 48 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Type));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 48 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.PropertyName));
            
            #line default
            #line hidden
            
            #line 48 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  if(!Model.Inpc){
            
            #line default
            #line hidden
            this.Write(" { get; ");
            
            #line 48 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(property.HasSetter){
            
            #line default
            #line hidden
            this.Write("set; ");
            
            #line 48 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("}");
            
            #line 48 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(property.HasDefaultValue){
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 48 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.DefaultValue));
            
            #line default
            #line hidden
            this.Write(";");
            
            #line 48 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 50 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
  }else{
            
            #line default
            #line hidden
            this.Write("\r\n    {\r\n        get { return ");
            
            #line 53 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.FieldName));
            
            #line default
            #line hidden
            this.Write("; }\r\n");
            
            #line 54 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(property.HasSetter){
            
            #line default
            #line hidden
            this.Write("        set \r\n        {\r\n            if (");
            
            #line 57 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.FieldName));
            
            #line default
            #line hidden
            this.Write(" != value)\r\n            {\r\n                ");
            
            #line 59 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.FieldName));
            
            #line default
            #line hidden
            this.Write(" = value; \r\n                RaisePropertyChanged();\r\n            }\r\n        }\r\n");
            
            #line 63 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    }\r\n");
            
            #line 65 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 67 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 68 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(Model.HasAdditionalPropertiesType){
            
            #line default
            #line hidden
            this.Write("    private System.Collections.Generic.IDictionary<string, ");
            
            #line 69 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.AdditionalPropertiesType));
            
            #line default
            #line hidden
            this.Write("> _additionalProperties = new System.Collections.Generic.Dictionary<string, ");
            
            #line 69 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.AdditionalPropertiesType));
            
            #line default
            #line hidden
            this.Write(">();\r\n\r\n    [Newtonsoft.Json.JsonExtensionData]\r\n    public System.Collections.Ge" +
                    "neric.IDictionary<string, ");
            
            #line 72 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.AdditionalPropertiesType));
            
            #line default
            #line hidden
            this.Write("> AdditionalProperties\r\n    {\r\n        get { return _additionalProperties; }\r\n   " +
                    "     set { _additionalProperties = value; }\r\n    }\r\n\r\n");
            
            #line 78 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 79 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(Model.Inpc){
            
            #line default
            #line hidden
            this.Write("    public event System.ComponentModel.PropertyChangedEventHandler PropertyChange" +
                    "d;\r\n\r\n");
            
            #line 82 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    public string ToJson() \r\n    {\r\n        return Newtonsoft.Json.JsonConvert.Se" +
                    "rializeObject(this");
            
            #line 85 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.JsonSerializerParameterCode));
            
            #line default
            #line hidden
            this.Write(");\r\n    }\r\n    \r\n    public static ");
            
            #line 88 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Class));
            
            #line default
            #line hidden
            this.Write(" FromJson(string data)\r\n    {\r\n        return Newtonsoft.Json.JsonConvert.Deseria" +
                    "lizeObject<");
            
            #line 90 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Class));
            
            #line default
            #line hidden
            this.Write(">(data");
            
            #line 90 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.JsonSerializerParameterCode));
            
            #line default
            #line hidden
            this.Write(");\r\n    }\r\n");
            
            #line 92 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
if(Model.Inpc){
            
            #line default
            #line hidden
            this.Write(@"
    protected virtual void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        var handler = PropertyChanged;
        if (handler != null) 
            handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }
");
            
            #line 100 "C:\work\my\ServiceLink\src\ServiceLink.Schema\Templates\CSharpClassTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    internal class CSharpClassTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}