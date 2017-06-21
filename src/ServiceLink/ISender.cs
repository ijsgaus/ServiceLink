using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    public interface ISender<TSource, TService>
        where TSource : IMessageSource
    {
        Task Fire<TMessage>(Expression<Func<TService, Action<TMessage>>> selector, TMessage message, CancellationToken token);
        
        void Publish<TMessage, TStore>(Expression<Func<TService, Action<TMessage>>> selector, TStore store,
            TMessage message)
            where TStore : IDeliveryStore;

        Guid Deliver<TMessage, TAnswer, TStore>(Expression<Func<TService, Func<TMessage, TAnswer>>> selector, TStore store, TMessage message, TimeSpan? resend)
            where TStore : IDeliveryStore;
    }
}