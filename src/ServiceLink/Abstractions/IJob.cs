using System;

namespace ServiceLink
{
    public interface IJob<out TArgs, in TResult>
    {
        TArgs Argument { get; }
        Action<TResult> Return { get; }
    }
}