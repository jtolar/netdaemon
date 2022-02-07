#region

using System.Threading.Channels;

#endregion

namespace NetDaemon.Extensions.ThrottleQueue;

public class ThrottleQueue : IThrottleQueue
{
    private readonly Channel<(Action, TimeSpan delay)> _actions = Channel.CreateBounded<(Action, TimeSpan delay)>(100);
    private readonly Task                              _processTask;

    public ThrottleQueue()
    {
        _processTask = ProcessAsync();
    }

    public async ValueTask DisposeAsync()
    {
        _actions.Writer.Complete();
        await _processTask.ConfigureAwait(false);
    }

    public void Post(Action action, TimeSpan timeSpan)
    {
        _actions.Writer.TryWrite(( action, timeSpan ));
    }

    private async Task ProcessAsync()
    {
        await foreach (var (action, timeSpan) in _actions.Reader.ReadAllAsync())
        {
            action();
            await Task.Delay(timeSpan).ConfigureAwait(false);
        }
    }
}