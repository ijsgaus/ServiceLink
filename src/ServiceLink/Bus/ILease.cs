using System;

namespace ServiceLink.Bus
{
    public interface ILease : IDisposable
    {
        TimeSpan LeaseInterval { get; }
        bool Renew();
    }
}