using System.Threading;
using ServiceLink;
using Contracts;

namespace MicroService1
{
    public class Commander<TSource, TStore>
        where TSource : IStoreHolder<TStore> 
        where TStore : IDeliveryStore
    {
        private readonly TSource _source; 
        
        private readonly IServiceLink<ICommandSource> _link;
        

        public Commander(IServiceLink<ICommandSource> link)
        {
            _link = link;
        
        }

        public void SendExec()
        {
            _link.EndPoint(p => p.Sample, _source).FireAsync(new SampleEvent());
        }

        public void SendExecute(TStore store) 
        {
            _link.EndPoint(p => p.SampleWithAnswer, _source).GetSender(store, new Command(), null);
                
        }
    }
}