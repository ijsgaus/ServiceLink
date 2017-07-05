using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Producer;
using ServiceLink.RabbitMq.Topology;
using ServiceLink.Serializers;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq.Channels
{
    internal class TransportOutChannel : ITransportOutChannel<byte[]>
    {
        private readonly Lazy<ILinkProducer> _lazyProducer;
        private readonly ILogger _logger;
        private readonly ProducerParams _params;
        
        public TransportOutChannel(ILogger logger, ILinkOwner linkOwner, ProducerParams @params)
        {
            _logger = logger;
            _params = @params;
            _lazyProducer = new Lazy<ILinkProducer>(() => linkOwner.GetOrAddProducer(_params));
        }
        
        public Func<CancellationToken, Task> GetSender(Serialized<byte[]> message)
        {
            var log = _logger.With("@channel", _params).With("@message", message);
            Func<CancellationToken, Task> Prepare()
            {
                var producer = _lazyProducer.Value;
                var publishParams = _params.PublishConfigure(message);
                
                Task Publish(CancellationToken cancellation)
                {
                    var task = producer.PublishAsync(message.Data, publishParams.MessageProperties,
                        publishParams.PublishProperties, cancellation);
                    task.LogResult(log, LogEvents.Publish);
                    return task;
                }

                return Publish;
            }

            return log.WithLog(Prepare, LogEvents.PreparePublish);
        }
    }
}