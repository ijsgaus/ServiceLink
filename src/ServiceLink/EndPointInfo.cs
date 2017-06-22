using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceLink
{
    public class EndPointInfo
    {
        public EndPointInfo([NotNull] string serviceName, [NotNull] string endpointName, [NotNull] TypeInfo messageType, TypeInfo answerType = null)
        {
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            EndpointName = endpointName ?? throw new ArgumentNullException(nameof(endpointName));
            MessageType = messageType ?? throw new ArgumentNullException(nameof(messageType));
            AnswerType = answerType;
        }

        [NotNull]
        public string ServiceName { get; }
        
        [NotNull]
        public string EndpointName { get; }
        
        [NotNull]
        public TypeInfo MessageType { get; }
        
        [CanBeNull]
        public TypeInfo AnswerType { get; }
    }
}
    