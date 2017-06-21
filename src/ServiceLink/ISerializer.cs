using ServiceLink.Monads;

namespace ServiceLink
{
    public interface ISerializer<TTarget>
    {
        ISerilized<TTarget> Serialize<TMessage>(TMessage message);
        Result<TMessage> TryDeserialize<TMessage>(ISerilized<TTarget> serilized);
    }

    public interface ISerilized<out TTarget>
    {
        ContentType ContentType { get; }
        EncodedType Type { get; }
        TTarget Data { get; }
    }

    public class EncodedType
    {
    }

    public class ContentType
    {
    }

    public interface ISerializerSelector<TTarget>
    {
        ISerializer<TTarget> Select<TMessage>(EndPointInfo info);
    }
}