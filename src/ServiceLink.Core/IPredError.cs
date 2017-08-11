namespace ServiceLink
{
    public interface IPredError<T>
    {
        string GetError(T value);
    }
}