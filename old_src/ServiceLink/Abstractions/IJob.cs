using System;

namespace ServiceLink
{
    public interface IJob<out TArgs, TResult>
    {
        TArgs Argument { get; }
        Action<Return<TResult>> Return { get; }
    }
}