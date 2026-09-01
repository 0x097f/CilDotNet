using System;
using System.Threading;
using System.Threading.Tasks;

namespace CilDotNet.Threading
{
    public sealed class CancellationInterrupt : IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private bool _disposed;

        public CancellationToken Token => _cts.Token;
        public bool IsRunning { get; private set; }
        public bool IsCancellationRequested => _cts.IsCancellationRequested;

        public event Action? OnCancelled;

        public CancellationInterrupt()
        {
            _cts = new CancellationTokenSource();
            IsRunning = true;
            Console.CancelKeyPress += OnCancelKeyPress!;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit!;
        }

        public void Cancel()
        {
            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                IsRunning = false;
                OnCancelled?.Invoke();
            }
        }

        public static bool WaitAll(params Task[] tasks)
        {
            try
            {
                return Task.WaitAll(tasks, TimeSpan.FromSeconds(5));
            }
            catch
            {
                return false;
            }
        }

        public Task Run(Action<CancellationToken> action)
        {
            return Task.Run(() => action(Token), Token);
        }

        public Task<T> Run<T>(Func<CancellationToken, T> func)
        {
            return Task.Run(() => func(Token), Token);
        }

        private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Cancel();
        }

        private void OnProcessExit(object? sender, EventArgs e)
        {
            Cancel();
        }

        public void Dispose()
        {
            if (_disposed) return;
            Console.CancelKeyPress -= OnCancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            _cts?.Cancel();
            _cts?.Dispose();
            _disposed = true;
            IsRunning = false;
        }
    }
}