using ServiceLink.Metadata;

namespace ServiceLink.Transport
{
    public interface ITranport
    {
        ITransportEventPoint<TMessage> EndPoint<TMessage>(IHolder holder, EndPointParams parameters);
    }
}