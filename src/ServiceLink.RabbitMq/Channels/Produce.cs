using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitLink.Producer;
using RabbitLink.Topology;

namespace ServiceLink.RabbitMq
{
    internal static class Produce
    {
        private static void OnPublishFinished<TMessage>(TMessage message, ILogger logger, Task task)
        {
            using (logger.BeginScope(message))
            {
                if (task.IsFaulted)
                    logger.LogError(0, task.Exception, "Publishing error");
                else if (task.IsCanceled)
                    logger.LogTrace("Cancelled");
                else
                    logger.LogTrace("Success");
            }
        }

        /// <summary>
        /// Carrieng only
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="logger">logger</param>
        /// <typeparam name="TMessage">message type</typeparam>
        /// <returns>carry version</returns>
        private static Action<Task> OnPublishFinished<TMessage>(TMessage message, ILogger logger)
            => t => OnPublishFinished(message, logger, t);

        public static Func<TMessage, Func<CancellationToken, Task>> PrepareSend<TMessage>(ILogger logger,
            ISerializer<byte[]> serializer,
            Lazy<ILinkProducer> producerLazy, Func<ProduceParams, ProduceParams> paramsCorrector)
            => message =>
            {
                var serialized = serializer.Serialize(message);
                var param = new ProduceParams();
                param.MessageProperties.ContentType = serialized.ContentType.ToString();
                param.MessageProperties.Type = serialized.Type.ToString();
                param.MessageProperties.TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                param = paramsCorrector(param);
                var producer = producerLazy.Value;
                return ct =>
                {
                    var task = producer.PublishAsync(serialized.Data, param.MessageProperties, param.PublishProperties,
                        ct);
                    task.ContinueWith(OnPublishFinished(message, logger), CancellationToken.None);
                    return task;
                };
            };

        public static Func<ILinkProducer> ProducerFactory(this ILinkOwner owner, string exchangeName, bool confirmMode,
            LinkExchangeType exchangeType)
            => () => owner.GetOrAddProducer(exchangeName, confirmMode, exchangeType);
    }

}