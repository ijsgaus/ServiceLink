using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Bus
{
    internal class MappedPublisher<TSource, TTarget> : IPublisher<TSource> 
    {
        private readonly IPublisher<TTarget> _targetPublisher;
        private readonly Func<TSource, TTarget> _mapper;

        public MappedPublisher(IPublisher<TTarget> targetPublisher, Func<TSource, TTarget> mapper)
        {
            _targetPublisher = targetPublisher ?? throw new ArgumentNullException(nameof(targetPublisher));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Func<CancellationToken, Task> PreparePublish(TSource message)
            => _targetPublisher.PreparePublish(_mapper(message));
            
    }
}