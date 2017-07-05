using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceLink.Endpoints
{
    public class CallableInfo : EndpointInfoBase
    {
        internal CallableInfo([NotNull] Type serviceType, [NotNull] MemberInfo member, [NotNull] Type parameterType,
            [NotNull] Type resultType) : base(serviceType, member)
        {
            ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            ResultType = resultType ?? throw new ArgumentNullException(nameof(resultType));
        }

        [NotNull]
        public Type ParameterType { get; }
        
        [NotNull]
        public Type ResultType { get; }
    }
}