using JetBrains.Annotations;

namespace ServiceLink.Endpoints
{
    public class NotifyEndpoint : Endpoint
    {
        public override EndpointInfoBase Info => Notify;
        [NotNull]
        public NotifyInfo Notify { get; }
        public NotifyObserveKind ObserveKind { get; }
        public string SubscribeName { get; }
        public ushort PrefetchCount { get; }
    }

    public enum NotifyObserveKind
    {
        PerName,
        PerSubscribe
    }
}