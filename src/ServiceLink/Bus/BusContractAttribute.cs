using System;

namespace ServiceLink.Bus
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BusContractAttribute : Attribute
    {
        public BusContractAttribute(string contractName) 
            => ContractName = contractName;

        public string ContractName { get; }
    }
}