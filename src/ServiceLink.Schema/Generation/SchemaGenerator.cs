using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using ServiceLink.Exceptions;
using ServiceLink.Markers;

namespace ServiceLink.Schema.Generation
{
    public class SchemaGenerator<T>
    {
        private readonly TypeInfo _serviceTypeInfo;

        public SchemaGenerator()
        {
            if(!typeof(T).GetTypeInfo().IsInterface) throw new ArgumentException($"Shema type must be interface");
            
            
            _serviceTypeInfo = typeof(T).GetTypeInfo();
        }

        public ServiceSchema Generate(SchemaGenerationOptions options = null)
        {
            options = options ?? new SchemaGenerationOptions();
            var serviceAttr = _serviceTypeInfo.GetCustomAttribute<ServiceAttribute>();
            if(serviceAttr == null)
                throw new ServiceInterfaceException($"Invalid service specification {typeof(T)} - no {nameof(ServiceAttribute)} specified");

            var serviceName = serviceAttr.Name ?? options.MemberNameToSchemaName?.Invoke(typeof(T).Name, true);
            if (serviceName == null)
                throw new ServiceInterfaceException($"Cannot determine service name of {typeof(T)}");

            

            var serviceSchema = new ServiceSchema
            {
                Version = serviceAttr.Version,
                Title = typeof(T).Name,
                Name = serviceName
                
            };

            
            var contracts = new List<Type>();
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var (name, schema, types) = GenerateEndpoint(propertyInfo, options, serviceSchema);
                serviceSchema.Endpoints.Add(name, schema);
                contracts.AddRange(types);
            }
            var containerGenerator = new ContainerClassBuilder(contracts.Distinct(),
                $"{typeof(T).Namespace}.{typeof(T).Name}Container");
            var containerType = containerGenerator.GenerateContainerType();

            var jsonSchema = JsonSchema4.FromTypeAsync(containerType, new JsonSchemaGeneratorSettings
            {
                SchemaProcessors = { new CustomSchemaProcessor() },

            }).Result;
            var containerJson = jsonSchema.ToJson();
            var containerJSchema = JObject.Parse(containerJson);
            serviceSchema.Contracts = containerJSchema;
            options.Extensions.Iter(p => p.ExtendService(typeof(T), serviceSchema));
            return serviceSchema;
        }

        private (string, EndpointSchema, IEnumerable<Type>) GenerateEndpoint(PropertyInfo property, SchemaGenerationOptions options, ServiceSchema serviceSchema)
        {
            var endpointAttr = property.GetCustomAttribute<EndpointAttribute>();
            var endpointName = endpointAttr?.Name ?? options.MemberNameToSchemaName?.Invoke(property.Name, false);
            if(endpointName == null)
                throw new ServiceInterfaceException($"Unknown endpoint name '{property.Name}' of {typeof(T)}");
            var propertyTypeInfo = property.PropertyType.GetTypeInfo();

            if (!propertyTypeInfo.IsGenericType)
                throw new ServiceInterfaceException($"Unknown property type '{property.Name}' of {typeof(T)}");
            var genericType = propertyTypeInfo.GetGenericTypeDefinition();
            EndpointSchema endpointSchema;
            List<Type> types = new List<Type>();
            if (genericType == typeof(IEvent<>))
            {
                var (type, schema) = GenerateContract(propertyTypeInfo.GenericTypeArguments[0], options);
                if(type != null)
                    types.Add(type);
                endpointSchema = new EventEndpointSchema
                {
                    
                    Event = schema
                };
            }
            else
            if (genericType == typeof(ICommand<>))
            {
                var (type, schema) = GenerateContract(propertyTypeInfo.GenericTypeArguments[0], options);
                if(type != null)
                    types.Add(type);
                endpointSchema = new CommandEndpointSchema
                {
                    Command = schema
                };
            }
            else
            if (genericType == typeof(ICallable<,>))
            {
                var (intype, inschema) = GenerateContract(propertyTypeInfo.GenericTypeArguments[0], options);
                if(intype != null) types.Add(intype);
                var (outtype, outschema) = GenerateContract(propertyTypeInfo.GenericTypeArguments[1], options);
                if (outtype != null) types.Add(outtype);
                endpointSchema = new CallableEndpointSchema
                {
                    Request = inschema,
                    Response = outschema
                };
            }
            else
                throw new ServiceInterfaceException($"Unknown property type '{property.Name}' of {typeof(T)}");
            endpointSchema.Title = property.Name;
            endpointSchema.Owner = serviceSchema;
            options.Extensions.Iter(p => p.ExtendEndpoint(property, endpointSchema));
            return (endpointName, endpointSchema, types);
        }

        private (Type, ContractTypeSchema) GenerateContract(Type contractType, SchemaGenerationOptions options)
        {
            if (contractType == options.UnitType || options.OtherUnitTypes.Contains(contractType))
                return (null, new WellKnownTypeSchema { Code = WellKnownTypes.UnitTypeCode, Title = contractType.Name });

            if (WellKnownTypes.CodeByType.ContainsKey(contractType))
                return (null, new WellKnownTypeSchema {Code = WellKnownTypes.CodeByType[contractType], Title = contractType.Name });
            
            var typeInfo = contractType.GetTypeInfo();
            if (typeInfo.IsArray)
            {
                var elementType = typeInfo.GetElementType();
                var (elType, elementSchema) = GenerateContract(elementType, options);
                return (elType, new ArrayTypeSchema
                {
                    Title = $"{elementType.Name}[]",
                    Element = elementSchema
                });
            }
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var elementType = typeInfo.GenericTypeArguments[0];
                var (elType, elementSchema) = GenerateContract(elementType, options);
                return (elType, new ArrayTypeSchema
                {
                    Title = $"IEnumerable<{elementType.Name}>",
                    Element = elementSchema
                });
            }

            var knownTypeAttributes = typeInfo.GetCustomAttributes<KnownTypeAttribute>().ToArray();
            if(typeInfo.IsAbstract && knownTypeAttributes.Length == 0)
                throw new ServiceInterfaceException($"Contract {contractType} is abstract, but not have KnownTypeAttribute");
            if(knownTypeAttributes.Length == 0)
                return GenerateObjectContract(contractType, options);
            var types = new List<Type>();
            foreach (var knownTypeAttribute in knownTypeAttributes)
            {
                if (knownTypeAttribute.MethodName != null)
                {
                    var method = contractType.GetMethod(knownTypeAttribute.MethodName);
                    var subTypes = (IEnumerable<Type>)  method.Invoke(null, new object[0]);
                    types.AddRange(subTypes);
                }
                else
                    types.Add(knownTypeAttribute.Type);
            }
            if(!typeInfo.IsAbstract)
                types.Add(contractType);
            return (contractType, new ObjectTypeSchema
            {
                Title = contractType.Name,
                TypeReference = contractType.Name
            });
        }

        private (Type,ObjectTypeSchema) GenerateObjectContract(Type contractType, SchemaGenerationOptions options)
        {
            var contractAttr = contractType.GetTypeInfo().GetCustomAttribute<ContractAttribute>();
            if (contractAttr == null)
                throw new ServiceInterfaceException(
                    $"Invalid astral contract '{contractType}' -  {nameof(ContractAttribute)} not found");
            var contractName = contractAttr.Name ?? options.MemberNameToSchemaName?.Invoke(contractType.Name, false);
            if (contractName == null)
                throw new ServiceInterfaceException($"Unknown contract name for '{contractType}");


            var contractSchema = new ObjectTypeSchema
            {
                Title = contractType.Name,
                TypeReference = contractType.Name
            };
            return (contractType, contractSchema);
        }

        
    }
}