using System;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using NetDaemon.AppModel;
using NetDaemon.Extensions.Logging;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.Extensions.ThrottleQueue;
using NetDaemon.Extensions.Tts;
using NetDaemon.Runtime;

#pragma warning disable CA1812

try
{
    await Host.CreateDefaultBuilder(args)
              .UseNetDaemonAppSettings()
              .UseNetDaemonDefaultLogging()
              .UseNetDaemonRuntime()
              .UseNetDaemonTextToSpeech()
              .UseNetDaemonMqttEntityManagement()
              .UseNetDaemonThrottleQueue()
              .ConfigureServices((_, services) =>
                  services
                      // change type of compilation here
                      // .AddAppsFromSource(true)
                      .AddAppsFromAssembly(Assembly.GetEntryAssembly()!)
                      // Remove this is you are not running the integration!
                      .AddNetDaemonStateManager()
              )
              .Build()
              .RunAsync()
              .ConfigureAwait(false);
}
catch (Exception e)
{
    Console.WriteLine($"Failed to start host... {e}");
    throw;
}