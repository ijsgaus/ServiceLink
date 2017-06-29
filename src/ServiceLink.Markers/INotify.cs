namespace ServiceLink.Markers
{
    public interface INotify<in TMessage>
    {
        T Configure<T>(T current, TMessage message);
    }
}