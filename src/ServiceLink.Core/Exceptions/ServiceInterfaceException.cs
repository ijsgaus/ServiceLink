using System;

namespace ServiceLink.Exceptions

{
    [Serializable]
    public class ServiceInterfaceException : Exception
    {
        
        public ServiceInterfaceException()
        {
        }

        public ServiceInterfaceException(string message) : base(message)
        {
        }

        public ServiceInterfaceException(string message, Exception inner) : base(message, inner)
        {
        }

        
    }
}