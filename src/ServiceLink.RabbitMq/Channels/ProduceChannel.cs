using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Producer;

namespace ServiceLink.RabbitMq
{
    internal class ProduceChannel<TMessage>
    {
        private readonly ISerializer<byte[]> _serializer;
        private readonly Lazy<ILinkProducer> _producer;
        private readonly Func<ProduceParams, ProduceParams> _paramsCorrector;
        private readonly ILogger _logger;

        public ProduceChannel(ILogger logger, ISerializer<byte[]> serializer,
            Lazy<ILinkProducer> producer, Func<ProduceParams, ProduceParams> paramsCorrector
           )
        {
            _serializer = serializer;
            _producer = producer;

            _paramsCorrector = paramsCorrector;
            _logger = logger;

        }

        private Action<Task> OnPublishFinished(TMessage message)
        {
            void After(Task task)
            {
                using (_logger.BeginScope(message))
                {
                    if (task.IsFaulted)
                        _logger.LogError(0, task.Exception, "Publishing error");
                    else if (task.IsCanceled)
                        _logger.LogTrace("Cancelled");
                    else
                        _logger.LogTrace("Success");
                }
            }

            return After;
        }

        public Func<CancellationToken, Task> PrepareSend(TMessage message)
        {
            var serialized = _serializer.Serialize(message);
            var param = new ProduceParams();
            param.MessageProperties.ContentType = serialized.ContentType.ToString();
            param.MessageProperties.Type = serialized.Type.ToString();
            param.MessageProperties.TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            param = _paramsCorrector(param);
            var producer = _producer.Value;
            return ct =>
            {
                var task = producer.PublishAsync(serialized.Data, param.MessageProperties, param.PublishProperties, ct);
                task.ContinueWith(OnPublishFinished(message), CancellationToken.None);
                return task;
            };
        }
    }
}