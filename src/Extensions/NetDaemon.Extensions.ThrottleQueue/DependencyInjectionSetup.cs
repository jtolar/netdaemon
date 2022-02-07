using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NetDaemon.Extensions.ThrottleQueue;

public static class DependencyInjectionSetup
{
    public static IHostBuilder UseNetDaemonThrottleQueue(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) => { services.AddSingleton<IThrottleQueue, ThrottleQueue>(); });
    }
}