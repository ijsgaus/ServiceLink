namespace ServiceLink.Markers
{
    public interface ICallable<in TArgs, in TResult>
    {
        T ConfigureRequest<T>(T current, TArgs args);
        T ConfigureResponse<T>(T current, TResult result);
    }
}