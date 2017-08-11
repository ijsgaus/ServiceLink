using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceLink.Endpoints
{
    public abstract class EndpointInfoBase
    {
        internal EndpointInfoBase([NotNull] Type serviceType, [NotNull] MemberInfo member)
        {
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            Member = member ?? throw new ArgumentNullException(nameof(member));
        }

        [NotNull]
        public Type ServiceType { get; }
        
        [NotNull]
        public MemberInfo Member { get; }

        
    }
}