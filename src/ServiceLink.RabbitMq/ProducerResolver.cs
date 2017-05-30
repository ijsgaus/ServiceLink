using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ServiceLink.RabbitMq
{
    internal class ProducerResolver : IProducerResolver
    {
        private readonly IReadOnlyCollection<IProducerResoveRule> _rules;

        public ProducerResolver(IEnumerable<IProducerResoveRule> rules)
        {
            _rules = new ReadOnlyCollection<IProducerResoveRule>(rules.OrderByDescending(p => p.Priority).ToList());
        }

        public IProducer GetTopology<T>(T message)
            => _rules.Select(p => p.GetProducer(message?.GetType() ?? typeof(T), message)).FirstOrDefault(p => p != null);
    }

    public interface IProducerResoveRule
    {
        int Priority { get; }
        IProducer GetProducer(Type messageType, object message);
    }
}