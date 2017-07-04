using System;
using System.Reflection;
using ServiceLink.Metadata;

namespace ServiceLink.Configuration
{
    public delegate EndPointParams LinkConfiguration(EndPointType endpointType, Type serviceType, Type argType,
        Type resType, MemberInfo member, IHolder holder, LinkConfigure config);
}