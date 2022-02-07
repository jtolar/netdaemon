namespace NetDaemon.Extensions.ThrottleQueue;

public interface IThrottleQueue
{
    void Post(Action action, TimeSpan timeSpan);
}