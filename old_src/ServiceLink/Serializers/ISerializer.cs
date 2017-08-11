namespace ServiceLink.Serializers
{
    public interface ISerializer<TTarget>
    {
        Serialized<TTarget> Serialize<TMessage>(TMessage message);
        Result<TMessage> TryDeserialize<TMessage>(Serialized<TTarget> serialized);
    }
}