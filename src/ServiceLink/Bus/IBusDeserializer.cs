using System;
using ServiceLink.Monads;

namespace ServiceLink.Bus
{
    public interface IBusDeserializer
    {
        Result<(Type, object)> Deserialize(SerializedMessage message);
    }
}