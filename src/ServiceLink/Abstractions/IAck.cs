using System;

namespace ServiceLink
{
    public interface IAck<out T>
    {
        T Message { get; }
        Action<Ack> Confirm { get; }
    }
}