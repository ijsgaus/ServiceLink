using JetBrains.Annotations;

namespace ServiceLink.Endpoints
{
    public abstract class Endpoint
    {
        
        
        [NotNull]
        public string Holder { get; }
        [CanBeNull]
        public string StoreSignature { get; }
        
        [NotNull]
        public string ServiceName { get; }
        [NotNull]
        public string EndpointName { get; }

        public abstract EndpointInfoBase Info { get; }

        
        
        
    }
}