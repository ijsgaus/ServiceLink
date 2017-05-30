using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Reflection;
using ServiceLink.Exceptions;

namespace ServiceLink.Bus
{
    /*
    public class AttributeContractResolver : IBusContractResolver
    {
        private readonly IBusContractResolver _fallBack;
        private static readonly ConcurrentDictionary<Type, string> Cache = new ConcurrentDictionary<Type, string>();

        private static string GetContract(Type type)
            => Cache.GetOrAdd(type, t => t.GetTypeInfo().GetCustomAttribute<BusContractAttribute>()?.ContractName);

        public AttributeContractResolver()
        {
        }

        public AttributeContractResolver(IBusContractResolver fallBack)
        {
            _fallBack = fallBack;
        }

        public string GetContract<T>(T value)
        {
            var type = value.GetType() ?? typeof(T);
            return GetContract(type) ??
                   (_fallBack != null
                       ? _fallBack.GetContract(value)
                       : throw new MissedContractException($"Contract attribute for type {type.FullName} not found")
                   );
        }
    }
    */
}