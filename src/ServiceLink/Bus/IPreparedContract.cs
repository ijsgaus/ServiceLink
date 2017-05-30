using System;

namespace ServiceLink.Bus
{
    public interface IPreparedContract
    {
        string Code { get; }
        Type BodyType { get; }
        object Body { get; }
    }
}