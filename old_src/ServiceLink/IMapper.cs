namespace ServiceLink
{
    public interface IMapper
    {
        TTarget Map<TSource, TTarget>(TSource source);
    }
}