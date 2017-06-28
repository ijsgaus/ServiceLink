using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceLink
{
    public interface ITranportProvider
    {
        
    }
    
    public class ServiceLinkManager 
    {
        internal IServiceLinkConfig Config { get; }
    }

    public interface IServiceLinkConfig
    {
        IServiceConfig<TService> GetService<TService>([CanBeNull] IServiceConfig<TService> previsios);
    }

    public interface IServiceConfig<TService>
    {
        
    }

    public interface IEventConfig<TService, TPayload>
    {
        
    }

    
    
    
    
    
}