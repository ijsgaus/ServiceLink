using System;

namespace ServiceLink.Bus
{
    public interface IContractExtractor
    {
        Type ResultType { get; }
        Type BodyType { get; }
        Func<object, object> BodyToResult { get; }
    }
}