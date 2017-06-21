using ServiceLink;
using Contracts;

namespace MicroService1
{
    public class Commander<TSource>
    {
        private readonly ISender<TSource, ICommandSource> _sender;

        public Commander(ISender<TSource, ICommandSource> sender)
        {
            _sender = sender;
        }

        public void SendExec()
        {
            _sender.Fire(p => p.Execute, new Command());
        }
    }
}