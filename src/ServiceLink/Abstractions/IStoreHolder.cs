using System;

namespace ServiceLink
{
    public interface IStoreHolder<out TStore> : IHolder where TStore : IDeliveryStore
    {
        
    }

    
        
}