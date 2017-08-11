using System;

namespace ServiceLink.Exceptions
{
    public class UnwrapException : Exception
    {
        public UnwrapException()
        {
        }

        public UnwrapException(string message) : base(message)
        {
        }

        public UnwrapException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}