using System;

namespace ServiceLink.Exceptions
{
    
    public class PredicateException : Exception
    {
     
        public PredicateException()
        {
        }

        public PredicateException(string message) : base(message)
        {
        }

        public PredicateException(string message, Exception inner) : base(message, inner)
        {
        }

    }
}