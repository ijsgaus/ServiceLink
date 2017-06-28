using System;
using System.Linq.Expressions;
using ServiceLink.Markers;

namespace ServiceLink
{
    public interface IServiceLink<TService>
        where TService : class
    {
        IEventPoint<TMessage> EndPoint<TMessage>(Expression<Func<TService, IEvent<TMessage>>> selector, IHolder holder);
        IEventPoint<TMessage, TStore> EndPoint<TMessage, TStore>(Expression<Func<TService, IEvent<TMessage>>> selector, IStoreHolder<TStore> holder) 
            where TStore : IDeliveryStore;

        ICommandPoint<TCommand> EndPoint<TCommand>(Expression<Func<TService, ICommand<TCommand>>> selector, IHolder holder);
        ICommandPoint<TCommand, TStore> EndPoint<TCommand, TStore>(Expression<Func<TService, ICommand<TCommand>>> selector, IStoreHolder<TStore> holder) 
            where TStore : IDeliveryStore;
        
        ICallablePoint<TArgs, TResult> EndPoint<TArgs, TResult>(Expression<Func<TService, ICallable<TArgs, TResult>>> selector, IHolder holder);
        ICallablePoint<TArgs, TResult, TStore> EndPoint<TArgs, TResult, TStore>(Expression<Func<TService, ICallable<TArgs, TResult>>> selector, IStoreHolder<TStore> holder) 
            where TStore : IDeliveryStore;
    }
    
}