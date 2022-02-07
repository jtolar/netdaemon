#region

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetDaemon.AppModel;
using NetDaemon.Extensions.ThrottleQueue;
using NetDaemon.HassModel;

#endregion

namespace DebugHost.apps.Extensions;

[NetDaemonApp]
[Focus]
public class ThrottleQueueApp : IAsyncInitializable
{
    private readonly IHaContext                _ha;
    private readonly ILogger<ThrottleQueueApp> _logger;
    private readonly IThrottleQueue            _queue;

    public ThrottleQueueApp(IHaContext ha, ILogger<ThrottleQueueApp> logger, IThrottleQueue queue)
    {
        _ha     = ha;
        _logger = logger;
        _queue  = queue;
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We need to log unexpected errors")]
    [SuppressMessage("Naming", "CA1727:Use PascalCase for named placeholders", Justification = "<Pending>")]
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Queue 5 messages with a 1 second delay");
            for (var i = 1; i <= 5; i++)
            {
                _logger.LogInformation("Queue messages: {i}", i);
                var messageId = i;
                _queue.Post(() => _logger.LogInformation("This is throttled message {i}", messageId), TimeSpan.FromSeconds(1));
            }

            await _queue.DisposeAsync().ConfigureAwait(false);
            _logger.LogInformation("Throttle queue demo complete");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }
}