using ServiceLink.Metadata;

namespace ServiceLink.Transport
{
    public interface ITranport
    {
        INotifyTransport<TMessage> EndPoint<TMessage>(EndPointParams parameters);
    }
}