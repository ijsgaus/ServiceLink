using System;

namespace ServiceLink.Bus
{
    public interface IPublishSafeStore
    {
        (SerializedMessage, ILease) GetAwaited();
        TimeSpan CheckInterval { get; }
    }
}