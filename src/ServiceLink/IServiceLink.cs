using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ServiceLink
{

    public interface ISender<TSource, TService>
        where TSource : IMessageSource
    {
        Task Fire<TMessage>(Expression<Func<TService, IEndPoint<TMessage>>> selector, TMessage message);

        void Publish<TMessage, TStore>(Expression<Func<TService, IEndPoint<TMessage>>> selector, TStore store,
            TMessage message);

        Guid Deliver<TMessage, TAnswer, TStore>(Expression<Func<TService, IEndPoint<TMessage, TAnswer>>> selector,
            TStore store, TMessage message);
    }

    public interface IMessageSource
    {
        string Application { get; }
    }


    public interface ITransport
    {
        IProducer<TMessage> GetOrAddProducer<TMessage>(EndPointInfo info);
    }

    public class EndPointInfo
    {
        public EndPointInfo(string serviceName, string endpointName)
        {
            ServiceName = serviceName;
            EndpointName = endpointName;
        }

        public string ServiceName { get; }
        public string EndpointName { get; }
    }
    

    public interface IProducer<in TMessage>
    {
        Task Publish(TMessage message, IMessageSource source);
    }

    public interface IMetaProvider<TService>
        where TService : class
    {
        string ServiceName { get; }
        string GetEndPointName(PropertyInfo endPoint);
    }

    internal class Sender<TSource, TService> : ISender<TSource, TService>
        where TService : class 
        where TSource : IMessageSource
    {
        private readonly TSource _source;
        private readonly IMetaProvider<TService> _metaProvider;
        private readonly ITransport _transport;

        public Sender(TSource source, IMetaProvider<TService> metaProvider, ITransport transport)
        {
            _source = source;
            _metaProvider = metaProvider;
            _transport = transport;
        }

        public Task Fire<TMessage>(Expression<Func<TService, IEndPoint<TMessage>>> selector, TMessage message)
        {
            var endPointInfo = new EndPointInfo(_metaProvider.ServiceName, _metaProvider.GetEndPointName(selector.GetProperty()));
            var producer = _transport.GetOrAddProducer<TMessage>(endPointInfo);
            return producer.Publish(message, _source);
        }

        public Guid Deliver<TMessage, TAnswer, TStore>(Expression<Func<TService, IEndPoint<TMessage, TAnswer>>> selector, TStore store, TMessage message)
        {
            throw new NotImplementedException();
        }
    }
}