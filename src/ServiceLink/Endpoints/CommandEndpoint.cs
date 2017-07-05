using JetBrains.Annotations;

namespace ServiceLink.Endpoints
{
    public class CommandEndpoint : ReplayEndpoint
    {
        public override EndpointInfoBase Info => Coommand;

        [NotNull]
        public CommandInfo Coommand { get; }
    }
}