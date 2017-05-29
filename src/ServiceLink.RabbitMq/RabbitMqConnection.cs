using System;
using System.Collections.Concurrent;
using RabbitLink;
using RabbitLink.Configuration;
using RabbitLink.Producer;

namespace ServiceLink.RabbitMq
{
    internal class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly string _name;
        private readonly Lazy<Link> _lazyLink;
        private readonly ConcurrentDictionary<string, ILinkProducer> _producers = new ConcurrentDictionary<string, ILinkProducer>();

        public RabbitMqConnection(string name, string url, Action<ILinkConfigurationBuilder> configure)
        {
            _name = name;
            _lazyLink = new Lazy<Link>(() => new Link(url, configure));
        }

        public ILinkProducer GetProducer(string name, Func<Link, ILinkProducer> factory)
            => _producers.GetOrAdd(name, _ => factory(_lazyLink.Value));


        public void Dispose()
        {
            if(_lazyLink.IsValueCreated)
                _lazyLink.Value.Dispose();
        }
    }
}