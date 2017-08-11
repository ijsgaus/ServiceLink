using ServiceLink.Schema.CSharp;

namespace ServiceLink.Schema.RabbitMq
{
    public class RabbitCSharpGenerator : ICSharpCodeGeneratorExtension
    {
        public void WriteNamespaces(IndentWriter writer, ServiceSchema schema)
        {
            writer.WriteLine("using ServiceLink.Markers.RabbitMq;");
        }

        public void WriteServiceAttributes(IndentWriter writer, ServiceSchema schema)
        {
            var serviceExt = new ServiceSchemaExtension(schema);
            WriteCommonAttributes(writer, serviceExt);
        }

        public void WriteEndpointAttributes(IndentWriter writer, EndpointSchema endpointValue)
        {
            var endpointEx = new EndpointSchemaExtension(endpointValue);
            WriteCommonAttributes(writer, endpointEx);
        }

        private static void WriteCommonAttributes(IndentWriter writer, SchemaExtension serviceExt)
        {
            if (serviceExt.ExchangeIn.IsSome && serviceExt.ExchangeInType.IsSome)
                writer.WriteLine(
                    $"[ExchangeIn(ExchangeType.{serviceExt.ExchangeInType.Unwrap()}, \"{serviceExt.ExchangeIn.Unwrap()}\")]");
            else if (serviceExt.ExchangeIn.IsSome)
                writer.WriteLine($"[ExchangeIn(name = \"{serviceExt.ExchangeIn.Unwrap()}\")]");
            else if (serviceExt.ExchangeInType.IsSome)
                writer.WriteLine($"[ExchangeIn(ExchangeType.{serviceExt.ExchangeInType.Unwrap()})]");
            if (serviceExt.ExchangeIn.IsSome && serviceExt.ExchangeOutType.IsSome)
                writer.WriteLine(
                    $"[ExchangeIn(ExchangeType.{serviceExt.ExchangeOutType.Unwrap()}, \"{serviceExt.ExchangeOut.Unwrap()}\")]");
            else if (serviceExt.ExchangeOut.IsSome)
                writer.WriteLine($"[ExchangeIn(name = \"{serviceExt.ExchangeOut.Unwrap()}\")]");
            else if (serviceExt.ExchangeOutType.IsSome)
                writer.WriteLine($"[ExchangeIn(ExchangeType.{serviceExt.ExchangeOutType.Unwrap()})]");

            serviceExt.RoutingKey.IfSome(p => writer.WriteLine($"[RoutingKey(\"{p}\")]"));
        }

        
    }
}