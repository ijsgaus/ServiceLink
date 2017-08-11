using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceLink.Endpoints
{
    public class NotifyInfo : EndpointInfoBase
    {
        internal NotifyInfo([NotNull] Type serviceType, [NotNull] MemberInfo member, [NotNull] Type messageType) : base(
            serviceType, member)
        {
            MessageType = messageType ?? throw new ArgumentNullException(nameof(messageType));
        }

        [NotNull]
        public Type MessageType { get; }
    }
}