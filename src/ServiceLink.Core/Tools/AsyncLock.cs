using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLink.Tools
{
    public class AsyncLock : IDisposable
    {
        private readonly SemaphoreSlim _slim = new SemaphoreSlim(1);

        public async Task<IDisposable> TakeAsync(CancellationToken token)
        {
            await _slim.WaitAsync(token);
            return Disposable.Create(() => _slim.Release());
        }

        public async Task<IDisposable> TakeAsync(TimeSpan timeout, CancellationToken token)
        {
            await _slim.WaitAsync(timeout, token);
            return Disposable.Create(() => _slim.Release());
        }

        public async Task<IDisposable> TakeAsync(TimeSpan timeout)
        {
            await _slim.WaitAsync(timeout);
            return Disposable.Create(() => _slim.Release());
        }

        public async Task<IDisposable> TakeAsync()
        {
            await _slim.WaitAsync();
            return Disposable.Create(() => _slim.Release());
        }


        public void Dispose()
        {
            _slim.Dispose();
        }

        ~AsyncLock()
        {
            Dispose();
        }
    }


}