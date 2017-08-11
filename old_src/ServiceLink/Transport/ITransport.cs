using ServiceLink.Endpoints;

namespace ServiceLink.Transport
{
    public interface ITransport<TFormat>
    {
        ITransportOutChannel<TFormat> GetOutChannel(Endpoint endpoint);
        ITransportInChannel<TFormat> GetInChannel(Endpoint endpoint);
    }
}