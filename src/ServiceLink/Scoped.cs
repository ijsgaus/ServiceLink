using System;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLink
{
    public class Scoped<T> : IDisposable
    {
        private readonly IServiceScope _scope;

        public Scoped(IServiceProvider provider)
        {
            _scope = provider?.CreateScope() ?? throw new ArgumentNullException(nameof(provider));
            Value = _scope.ServiceProvider.GetRequiredService<T>();
        }

        public T Value { get; }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}