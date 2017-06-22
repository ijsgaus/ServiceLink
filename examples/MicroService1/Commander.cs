using System.Threading;
using ServiceLink;
using Contracts;

namespace MicroService1
{
    public class Commander<TSource>
        where TSource : ILinkStakeHolder
    {
        private readonly IServiceLink<TSource, ICommandSource> _link;
        

        public Commander(IServiceLink<TSource, ICommandSource> link)
        {
            _link = link;
        
        }

        public void SendExec()
        {
            _link.EndPoint(p => p.Sample).FireAsync(new SampleEvent());
        }

        public void SendExecute<TStore>(TStore store) where TStore : IDeliveryStore
        {
            _link.EndPoint(p => p.SampleWithAnswer).Publish(store, new Command());
        }
    }
}