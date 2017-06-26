
using System;

namespace ServiceLink
{
    public interface IEndPoint<TMessage> : IEndPoint<TMessage, ValueTuple>
    {
        
    }

    public interface IEndPoint<TMessage, TAnswer>
    {
        
    }
}