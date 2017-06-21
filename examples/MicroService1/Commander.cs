using System.Threading;
using ServiceLink;
using Contracts;

namespace MicroService1
{
    public class Commander<TSource>
        where TSource : IMessageSource
    {
        private readonly ISender<TSource, ICommandSource> _sender;

        public Commander(ISender<TSource, ICommandSource> sender)
        {
            _sender = sender;
        }

        public void SendExec()
        {
            _sender.Fire(p => p.Execute, new Command(), CancellationToken.None);
        }

        public void SendExecute<TStore>(TStore store) where TStore : IDeliveryStore
        {
            _sender.Deliver(p => p.Exec, store, new Command(), null);
        }
    }
}