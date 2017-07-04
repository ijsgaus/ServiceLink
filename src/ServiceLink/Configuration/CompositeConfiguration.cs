using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceLink.Metadata;
using ServiceLink.Serializers;

namespace ServiceLink.Configuration
{
    internal class CompositeConfiguration 
    {
        private LinkConfigure _linkConfiguration;
        private readonly Dictionary<Type, LinkConfigure> _typeConfigurations = new Dictionary<Type, LinkConfigure>();
        private readonly Dictionary<(Type, string), LinkConfigure> _endpointConfigurations = new Dictionary<(Type, string), LinkConfigure>();

        private static EndPointParams DefaultConfigure(EndPointType endpointType, Type serviceType, Type argType,
            Type resType, MemberInfo member, IHolder holder)
        {
            return new EndPointParams(endpointType, holder, serviceType.Name, member.Name, member, new JsonUtf8Serializer());
        }

        public CompositeConfiguration(LinkConfigure linkConfigure)
        {
            _linkConfiguration = linkConfigure ?? DefaultConfigure;
        }


        public void Register(LinkConfiguration configuration)
        {
            _linkConfiguration = (ep, serviceType, argType, resType, mi, holder) =>
                configuration(ep, serviceType, argType, resType, mi, holder, _linkConfiguration);
        }

        public void Register<TService>(LinkConfiguration configuration)
        {
            if (_typeConfigurations.TryGetValue(typeof(TService), out var current))
                _typeConfigurations[typeof(TService)] = (ep, serviceType, argType, resType, mi, holder) =>
                    configuration(ep, serviceType, argType, resType, mi, holder, current);
            else
                _typeConfigurations[typeof(TService)] = (ep, serviceType, argType, resType, mi, holder) =>
                    configuration(ep, serviceType, argType, resType, mi, holder, _linkConfiguration);
        }
        
        public void Register<TService>(MemberInfo member, LinkConfiguration configuration)
        {
            if (_endpointConfigurations.TryGetValue((typeof(TService), member.Name), out var current))
                _endpointConfigurations[(typeof(TService), member.Name)] = (ep, serviceType, argType, resType, mi, holder) =>
                    configuration(ep, serviceType, argType, resType, mi, holder, current);
            else
            if (_typeConfigurations.TryGetValue(typeof(TService), out var current1))
                _endpointConfigurations[(typeof(TService), member.Name)] = (ep, serviceType, argType, resType, mi, holder) =>
                    configuration(ep, serviceType, argType, resType, mi, holder, current1);
            else
                _endpointConfigurations[(typeof(TService), member.Name)] = (ep, serviceType, argType, resType, mi, holder) =>
                    configuration(ep, serviceType, argType, resType, mi, holder, _linkConfiguration);
        }

        public EndPointParams Configure(EndPointType endpointType, Type serviceType, Type argType,
            Type resType, MemberInfo member, IHolder holder)
        {
            if (_endpointConfigurations.TryGetValue((serviceType, member.Name), out var current))
                return current(endpointType, serviceType, argType, resType, member, holder);
            if (_typeConfigurations.TryGetValue(serviceType, out var tc))
                return tc(endpointType, serviceType, argType, resType, member, holder);
            return _linkConfiguration(endpointType, serviceType, argType, resType, member, holder);
        }
        
    }

    
}