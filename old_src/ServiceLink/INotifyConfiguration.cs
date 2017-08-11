using ServiceLink.Configuration;

namespace ServiceLink
{
    public interface INotifyConfiguration<TService, TMessage>
    {
        void Configure(LinkConfiguration configuration);
    }
}