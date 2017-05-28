using System;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLink
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider StartWorkers(this IServiceProvider serviceProvider)
        {
            serviceProvider.GetServices<IWorker>();
            return serviceProvider;
        }

        
    }
}