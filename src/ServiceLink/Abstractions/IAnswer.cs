namespace ServiceLink
{
    public interface IAnswer<out TArgs, TResult, out TStore>
    {
        TArgs Argument { get; }
        Result<TResult> Result { get; }
        TStore Store { get; }
    }
}