namespace ServiceLink.Markers
{
    public interface IEvent<in TMessage>
    {
        T Configure<T>(T current, TMessage message);
    }
}