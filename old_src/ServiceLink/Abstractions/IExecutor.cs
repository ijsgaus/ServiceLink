using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink
{
    public interface IExecutor
    {
        Task Execute(Func<CancellationToken, Task> task, CancellationToken token);
    }
}