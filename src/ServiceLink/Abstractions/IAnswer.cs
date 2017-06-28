namespace ServiceLink
{
    public interface IAnswer<out TArgs, out TResult, out TStore>
    {
        TArgs Argument { get; }
        TResult Result { get; }
        TStore Store { get; }
    }
}