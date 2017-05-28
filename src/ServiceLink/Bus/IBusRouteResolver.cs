namespace ServiceLink.Bus
{
    public interface IBusRouteResolver
    {
        object GetRoute<T>(T value);
    }
}