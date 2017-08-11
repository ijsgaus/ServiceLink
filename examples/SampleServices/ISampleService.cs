using ServiceLink.Markers;
using ServiceLink.Markers.RabbitMq;

namespace SampleServices
{
    [Service("0.5", "sample.service")]
    [ExchangeOut(ExchangeType.Topic)]
    public interface ISampleService
    {
        [Endpoint("awesome.event")]
        [ExchangeIn(ExchangeType.Fanout, "topic.exchange")]
        IEvent<SampleEvent> AwesomeEvent { get; }

        [Endpoint("order.change")]
        [RoutingKey("special.routing.key")]
        ICommand<SampleCommand> OrderChange { get; }

        [Endpoint("an.array")]
        IEvent<int[]> AnArray { get; }

        [Endpoint("convert")]
        ICallable<int, string> Convert { get; }
    }
}