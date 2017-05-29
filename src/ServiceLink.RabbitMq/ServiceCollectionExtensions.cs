using System;
using Microsoft.Extensions.DependencyInjection;
using RabbitLink.Configuration;

namespace ServiceLink.RabbitMq
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseRabbitMqConnection(this IServiceCollection collection, string url, Action<ILinkConfigurationBuilder> configure)
        {
            collection.AddSingleton<IRabbitMqConnection>(_ => new RabbitMqConnection("", url, configure));
            return collection;
        }


    }
}