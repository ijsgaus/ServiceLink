namespace ServiceLink.Bus
{
    public interface IBusContractResolver
    {
        string GetContract<T>(T value);
    }
}