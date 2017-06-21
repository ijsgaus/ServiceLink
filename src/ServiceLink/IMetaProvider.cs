using System.Reflection;

namespace ServiceLink
{
    public interface IMetaProvider<TService>
        where TService : class
    {
        string ServiceName { get; }
        string GetEndPointName(MethodInfo endPoint);
    }
}