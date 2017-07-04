using System;
using System.Linq.Expressions;
using ServiceLink.Markers;

namespace ServiceLink
{
    public interface IServiceConfigure<TService>
    {
        INotifyConfiguration<TService, TMessage> Endpoint<TMessage>(
            Expression<Func<TService, INotify<TMessage>>> selector);
    }
}