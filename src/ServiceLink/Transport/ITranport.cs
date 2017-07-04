using ServiceLink.Metadata;

namespace ServiceLink.Transport
{
    public interface ITranport
    {
        INotifyPoint<TMessage> EndPoint<TMessage>(EndPointParams parameters);
    }
}