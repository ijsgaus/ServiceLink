using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceLink
{
    public abstract class AttributeMetaProvider<TServiceAttribute, TMethodAttribute, TService> : IMetaProvider<TService> 
        where TService : class
        where TServiceAttribute : Attribute
        where TMethodAttribute : Attribute 
    {
        private readonly Func<TMethodAttribute, string> _endPointExtractor;

        protected AttributeMetaProvider([NotNull] Func<TServiceAttribute, string> serviceExtractor,
            [NotNull] Func<TMethodAttribute, string> endPointExtractor)
        {
            if (serviceExtractor == null) throw new ArgumentNullException(nameof(serviceExtractor));
            _endPointExtractor = endPointExtractor ?? throw new ArgumentNullException(nameof(endPointExtractor));
            var serviceAttr = typeof(TService).GetTypeInfo().GetCustomAttribute<TServiceAttribute>();
            if (serviceAttr == null)
                throw new ArgumentException($"Type {typeof(TService)} not have attribute {typeof(TServiceAttribute)}");
            ServiceName = serviceExtractor(serviceAttr);
        }

        public string ServiceName { get; }

        public string GetEndPointName(MethodInfo endPoint)
        {
            var methodAttr = endPoint.GetCustomAttribute<TMethodAttribute>();
            if (methodAttr == null)
                throw new ArgumentException(
                    $"Method {endPoint} of type {typeof(TService)} not have attribute {typeof(TMethodAttribute)}");
            return _endPointExtractor(methodAttr);
        }
    }
}