namespace ServiceLink
{
    public interface ITransport
    {
        IProducer<TMessage> GetOrAddProducer<TMessage>(EndPointInfo info);
    }
}