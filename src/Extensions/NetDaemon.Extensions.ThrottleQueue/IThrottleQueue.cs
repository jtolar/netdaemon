namespace NetDaemon.Extensions.ThrottleQueue;

public interface IThrottleQueue : IAsyncDisposable
{
    void Post(Action action, TimeSpan timeSpan);
}