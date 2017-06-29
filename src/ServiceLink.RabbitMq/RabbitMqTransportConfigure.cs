using System.Collections.Generic;
using System.Linq;
using ServiceLink.Metadata;

namespace ServiceLink.RabbitMq
{
    internal class RabbitMqTransportConfigure : IEndpointConfigure
    {
        private readonly IReadOnlyCollection<IEndpointConfigure> _serviceConfigurers;
        private readonly IReadOnlyCollection<IEndpointConfigure> _endPointsConfigurers;

        public EventParameters ConfigureEvent(EventParameters parameters, EndPointParams info)
        {
            parameters = _serviceConfigurers.Aggregate(parameters, (c, p) => p.ConfigureEvent(c, info));
            return _endPointsConfigurers.Aggregate(parameters, (c, p) => p.ConfigureEvent(c, info));
        }

        public CommandParameters ConfigureCommand(CommandParameters parameters, EndPointParams info)
        {
            parameters = _serviceConfigurers.Aggregate(parameters, (c, p) => p.ConfigureCommand(c, info));
            return _endPointsConfigurers.Aggregate(parameters, (c, p) => p.ConfigureCommand(c, info));
        }
    }

    
}