using System;
using System.Linq.Expressions;
using ServiceLink.Configuration;
using ServiceLink.Markers;

namespace ServiceLink
{
    
    
    public interface IServiceLink<TService>
        where TService : class
    {
        INotifyPoint<TMessage> EndPoint<TMessage>(Expression<Func<TService, INotify<TMessage>>> selector, IHolder holder);
        INotifyPoint<TMessage, TStore> EndPoint<TMessage, TStore>(Expression<Func<TService, INotify<TMessage>>> selector, IStoreHolder<TStore> holder) 
            where TStore : IDeliveryStore;

        ICommandPoint<TCommand> EndPoint<TCommand>(Expression<Func<TService, ICommand<TCommand>>> selector, IHolder holder);
        ICommandPoint<TCommand, TStore> EndPoint<TCommand, TStore>(Expression<Func<TService, ICommand<TCommand>>> selector, IStoreHolder<TStore> holder) 
            where TStore : IDeliveryStore;
        
        ICallablePoint<TArgs, TResult> EndPoint<TArgs, TResult>(Expression<Func<TService, ICallable<TArgs, TResult>>> selector, IHolder holder);
        ICallablePoint<TArgs, TResult, TStore> EndPoint<TArgs, TResult, TStore>(Expression<Func<TService, ICallable<TArgs, TResult>>> selector, IStoreHolder<TStore> holder) 
            where TStore : IDeliveryStore;
    }

    public interface IServiceLink
    {
        IServiceLink<TService> GetService<TService>() where TService : class;
        void Configure(LinkConfiguration configuration);
        void Configure<TService>(LinkConfiguration configuration);
        IServiceConfigure<TService> Configure<TService>();
    }
}