using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceLink.Endpoints
{
    public class CommandInfo : EndpointInfoBase
    {
        internal CommandInfo([NotNull] Type serviceType, [NotNull] MemberInfo member, [NotNull] Type commandType) :
            base(serviceType, member)
        {
            CommandType = commandType ?? throw new ArgumentNullException(nameof(commandType));
        }

        [NotNull]
        public Type CommandType { get; }
    }
}