namespace ServiceLink.Markers
{
    public interface ICommand<in TCommand>
    {
        T Configure<T>(T current, TCommand command);
    }
}