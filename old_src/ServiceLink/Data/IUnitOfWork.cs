using System;
using System.Data;


namespace ServiceLink.Data
{
    public interface IUnitOfWork<out IContext>
    {
        void Execute(Action<IContext> execute, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        T Execute<T>(Func<IContext, T> execute, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
    
    
}