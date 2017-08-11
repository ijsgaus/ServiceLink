using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceLink.Endpoints;
using ServiceLink.Serializers;

namespace ServiceLink.Configuration
{
    public class ServiceLinkConfiguration 
    {
        private LinkConfigure _linkConfiguration;
        private readonly Dictionary<Type, LinkConfigure> _typeConfigurations = new Dictionary<Type, LinkConfigure>();
        private readonly Dictionary<(Type, string), LinkConfigure> _endpointConfigurations = new Dictionary<(Type, string), LinkConfigure>();

        private static LinkEndpointConfig DefaultConfigure(EndpointInfoBase endpoint, IHolder holder)
        {
            return new LinkEndpointConfig(endpoint.ServiceType.Name, endpoint.Member.Name);
        }

        public ServiceLinkConfiguration(LinkConfigure linkConfigure)
        {
            _linkConfiguration = linkConfigure ?? DefaultConfigure;
            
        }


        public void Register(LinkConfiguration configuration)
        {
            _linkConfiguration = (ep, holder) =>
                configuration(ep, holder, _linkConfiguration);
        }

        public void Register<TService>(LinkConfiguration configuration)
        {
            if (_typeConfigurations.TryGetValue(typeof(TService), out var current))
                _typeConfigurations[typeof(TService)] = (ep, holder) =>
                    configuration(ep, holder, current);
            else
                _typeConfigurations[typeof(TService)] = (ep, holder) =>
                    configuration(ep, holder, _linkConfiguration);
        }
        
        public void Register<TService>(MemberInfo member, LinkConfiguration configuration)
        {
            if (_endpointConfigurations.TryGetValue((typeof(TService), member.Name), out var current))
                _endpointConfigurations[(typeof(TService), member.Name)] = (ep, holder) =>
                    configuration(ep, holder, current);
            else
            if (_typeConfigurations.TryGetValue(typeof(TService), out var current1))
                _endpointConfigurations[(typeof(TService), member.Name)] = (ep, holder) =>
                    configuration(ep, holder, current1);
            else
                _endpointConfigurations[(typeof(TService), member.Name)] = (ep, holder) =>
                    configuration(ep, holder, _linkConfiguration);
        }

        public LinkEndpointConfig Configure(EndpointInfoBase endpoint, IHolder holder)
        {
            if (_endpointConfigurations.TryGetValue((endpoint.ServiceType, endpoint.Member.Name), out var current))
                return current(endpoint, holder);
            if (_typeConfigurations.TryGetValue(endpoint.ServiceType, out var tc))
                return tc(endpoint, holder);
            return _linkConfiguration(endpoint, holder);
        }
        
    }

    
}