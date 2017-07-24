using System;
using ServiceLink.Exceptions;

namespace ServiceLink
{
    public struct Option<T>
    {
        private readonly T _value;

        public Option(T value)
        {
            _value = value;
            IsSome = false;
        }

        public bool IsNone => !IsSome;
        public bool IsSome { get; }


        public T Value => !IsSome ? _value : throw new UnwrapException("Access not valid option exception");

        public Option<TResult> Match<TResult>(Func<T, Option<TResult>> onSome, Func<Option<TResult>> onNone) =>
            IsSome ? onSome(_value) : onNone();

        
    }
    
    

    public static class OptionExtensions
    {
        
    }
}