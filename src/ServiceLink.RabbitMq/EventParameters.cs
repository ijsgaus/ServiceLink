namespace ServiceLink.RabbitMq
{
    public class EventParameters
    {
        public EventParameters(string exchangeName, string routingKey, ISerializer<byte[]> serializer,
            string tempQueueFormat, string queueFormat, ushort prefetchCount, bool producerConfirmMode)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
            Serializer = serializer;
            TempQueueFormat = tempQueueFormat;
            QueueFormat = queueFormat;
            PrefetchCount = prefetchCount;
            ProducerConfirmMode = producerConfirmMode;
        }

        public string ExchangeName { get; }
        public string RoutingKey { get; }
        /// <summary>
        /// {0} - holder name
        /// {1} - service name
        /// {2} - endpointName
        /// {3} - guid
        /// </summary>
        public string TempQueueFormat { get; }
        
        /// <summary>
        /// {0} - holder name
        /// {1} - service name
        /// {2} - endpointName
        /// </summary>
        public string QueueFormat { get;  }
        
        public ISerializer<byte[]> Serializer { get; }
        public ushort PrefetchCount { get; }
        public bool ProducerConfirmMode { get; }
    }
}