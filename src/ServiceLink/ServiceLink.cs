using System;
using System.Linq.Expressions;
using System.Reflection;
using ServiceLink.Configuration;
using ServiceLink.Markers;

namespace ServiceLink
{
    public class ServiceLink : IServiceLink
    {
        private readonly CompositeConfiguration _configuration;
        
        public ServiceLink(LinkConfigure configure)
        {
            _configuration = new CompositeConfiguration(configure);
        }

        public IServiceLink<TService> GetService<TService>() where TService : class
        {
            throw new NotImplementedException();
        }

        public void Configure(LinkConfiguration configuration)
        {
            _configuration.Register(configuration);
        }

        public void Configure<TService>(LinkConfiguration configuration)
        {
            _configuration.Register<TService>(configuration);
        }

        public IServiceConfigure<TService> Configure<TService>()
        {
            return new ServiceConfigure<TService>(_configuration);
        }

        private class ServiceConfigure<TService> : IServiceConfigure<TService>
        {
            private readonly CompositeConfiguration _configuration;

            public ServiceConfigure(CompositeConfiguration configuration)
            {
                _configuration = configuration;
            }

            public INotifyConfiguration<TService, TMessage> Endpoint<TMessage>(Expression<Func<TService, INotify<TMessage>>> selector)
            {
                return new NotifyConfiguration<TService, TMessage>(_configuration, selector.GetProperty());
            }
        }

        private class NotifyConfiguration<TService, TMessage> : INotifyConfiguration<TService, TMessage>
        {
            private readonly CompositeConfiguration _configuration;
            private readonly MemberInfo _memberInfo;

            public NotifyConfiguration(CompositeConfiguration configuration, MemberInfo memberInfo)
            {
                _configuration = configuration;
                _memberInfo = memberInfo;
            }

            public void Configure(LinkConfiguration configuration)
            {
                _configuration.Register<TService>(_memberInfo, configuration);
            }
        }
    }
}