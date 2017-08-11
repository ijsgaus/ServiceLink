using JetBrains.Annotations;

namespace ServiceLink.Endpoints
{
    public class CallableEndpoint : ReplayEndpoint
    {
        public override EndpointInfoBase Info => Callable;
        [NotNull]
        public CallableInfo Callable { get; }
    }
}