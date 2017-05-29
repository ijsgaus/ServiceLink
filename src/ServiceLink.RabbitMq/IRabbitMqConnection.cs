using System;
using RabbitLink;
using RabbitLink.Producer;

namespace ServiceLink.RabbitMq
{
    public interface IRabbitMqConnection : IDisposable
    {
        ILinkProducer GetProducer(string name, Func<Link, ILinkProducer> factory);
    }
}