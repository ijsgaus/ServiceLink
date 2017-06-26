using System;

namespace ServiceLink.Exceptions
{
    
    public class ServiceLinkException : Exception
    {
    
        public ServiceLinkException()
        {
        }

        public ServiceLinkException(string message) : base(message)
        {
        }

        public ServiceLinkException(string message, Exception inner) : base(message, inner)
        {
        }

    
    }
}