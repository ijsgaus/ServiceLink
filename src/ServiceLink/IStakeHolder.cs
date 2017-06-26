using System;

namespace ServiceLink
{
    public interface IStakeHolder<out TStore>
        where TStore : IDeliveryStore
    {
        string Name { get; }
        TStore CreateStore();
    }

    public abstract class StakeHolder<TStore> : IStakeHolder<TStore>
        where TStore : IDeliveryStore
    {
        private readonly Func<TStore> _storeFactory;

        protected StakeHolder(string name, Func<TStore> storeFactory)
        {
            _storeFactory = storeFactory;
            Name = name;
        }

        public string Name { get; }

        TStore IStakeHolder<TStore>.CreateStore() => _storeFactory();
    }
        
}