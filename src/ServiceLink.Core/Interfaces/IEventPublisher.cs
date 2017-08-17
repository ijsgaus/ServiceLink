using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ServiceLink.Markers;

namespace ServiceLink.Interfaces
{
    public interface IEventPublisher<TService>
    {
        Task PublishAsync<TEvent>(Expression<Func<TService, IEvent<TEvent>>> selector, TEvent payload);
    }
}