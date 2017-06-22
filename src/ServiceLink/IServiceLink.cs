using System;
using System.Linq.Expressions;

namespace ServiceLink
{
    public interface IServiceLink<THolder, TService>
        where THolder : ILinkStakeHolder 
        where TService : class
    {
        IEndPoint<TMessage> EndPoint<TMessage>(Expression<Func<TService, EventHandler<TMessage>>> selector);
            
        IEndPoint<TMessage, TAnswer> EndPoint<TMessage, TAnswer>(Expression<Func<TService, Func<TMessage, TAnswer>>> selector);
    }
}