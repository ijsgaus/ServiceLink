using System;
using System.Threading.Tasks;

namespace ServiceLink.Bus
{
    public interface IBusContractResolver
    {
        IPreparedContract PrepareContract(Type type, object value);
        IContractExtractor GetExtractor(string contract);
    }
}