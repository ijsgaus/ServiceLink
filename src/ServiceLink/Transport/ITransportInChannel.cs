using System;

namespace ServiceLink.Transport
{
    public interface ITransportInChannel<T> : IObservable<IRecieveContext<T>>
    {
        
    }
}