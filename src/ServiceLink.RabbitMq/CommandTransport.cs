using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Producer;
using RabbitLink.Topology;
using ServiceLink.Metadata;
using ServiceLink.Transport;

namespace ServiceLink.RabbitMq
{
    internal class CommandTransport<TCommand> : ICommandTransport<TCommand>
    {
        private readonly Func<TCommand, Func<CancellationToken, Task>> _produceCommand;
        //private readonly Func<

        public CommandTransport(ILogger logger, IEndpointConfigure configure,
            ILinkOwner owner, EndPointParams endPoint)
        {
            var prm = new CommandParameters(endPoint.ServiceName, endPoint.EndpointName, endPoint.Serializer, 
                "{1}.{2}.{0}", 1);
            prm = configure.ConfigureCommand(prm, endPoint);
            _produceCommand = Produce.PrepareSend<TCommand>(logger, prm.Serializer, 
                new Lazy<ILinkProducer>(owner.ProducerFactory(prm.ExchangeName, true, LinkExchangeType.Direct)),
                p =>
                {
                    p.MessageProperties.AppId = endPoint.HolderName;
                    if (endPoint.DeliveryId != null)
                    {
                        p.MessageProperties.CorrelationId = endPoint.DeliveryId.Value.ToString("D");
                        p.MessageProperties.ReplyTo = endPoint.HolderName;
                    }
                    p.PublishProperties.RoutingKey = prm.RoutingKey;
                    return p;
                });
            
        }

        public Func<CancellationToken, Task> PrepareSend(TCommand message)
            => _produceCommand(message);

        public IObservable<IAck<TCommand>> Connect()
        {
            throw new NotImplementedException();
        }

        public IObservable<IAck<Unit>> GetAnswer()
        {
            throw new NotImplementedException();
        }
    }

    public class CommandParameters
    {
        public CommandParameters(string exchangeName, string routingKey, ISerializer<byte[]> serializer,
            string queueFormat, ushort prefetchCount)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
            Serializer = serializer;
            
            QueueFormat = queueFormat;
            PrefetchCount = prefetchCount;
        }

        public string ExchangeName { get; }
        public string RoutingKey { get; }
        
        /// <summary>
        /// {0} - holder name
        /// {1} - service name
        /// {2} - endpointName
        /// </summary>
        public string QueueFormat { get;  }
        
        public ISerializer<byte[]> Serializer { get; }
        public ushort PrefetchCount { get; }
   
    }
}