using System;

namespace ServiceLink.Bus
{
    public interface IBusDeserializer
    {
        (Type, object) Deserialize(SerializedMessage message);
    }
}