using ServiceLink.Serializers;

namespace ServiceLink.Transport
{
    public interface IRecieveContext<T>
    {
        Serialized<T> Message { get; }
    }
}