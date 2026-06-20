using DotNetODE.Business.DTO;
using DotNetODE.Business.Interactor;

namespace DotNetODE.Gateway.MVVM;

public abstract class BaseViewModel
{
    private readonly Dictionary<string, List<Delegate>> _observers = new();

    protected Channel<Output<TResult>> CreateChannel<TResult>(string name) => new(name);

    public IDisposable Observe<TResult>(Channel<Output<TResult>> channel, Action<Output<TResult>> observer)
    {
        if (!_observers.TryGetValue(channel.Name, out var listeners))
        {
            listeners = new List<Delegate>();
            _observers[channel.Name] = listeners;
        }

        listeners.Add(observer);
        return new ObservationToken(() => listeners.Remove(observer));
    }

    public void ClearObservers() => _observers.Clear();

    protected void Publish<TResult>(Channel<Output<TResult>> channel, Output<TResult> output)
    {
        if (!_observers.TryGetValue(channel.Name, out var listeners))
        {
            return;
        }

        foreach (var listener in listeners.OfType<Action<Output<TResult>>>().ToArray())
        {
            listener(output);
        }
    }

    protected async Task<Output<TResult>> DispatchUseCaseAsync<TParam, TResult>(
        TParam param,
        UseCase<TParam, TResult> useCase,
        Channel<Output<TResult>> channel)
    {
        var output = await UseCaseDispatcher.DispatchAsync(useCase, param).ConfigureAwait(false);
        Publish(channel, output);
        return output;
    }

    private sealed class ObservationToken : IDisposable
    {
        private readonly Action _dispose;
        private bool _disposed;

        public ObservationToken(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _dispose();
            _disposed = true;
        }
    }
}
