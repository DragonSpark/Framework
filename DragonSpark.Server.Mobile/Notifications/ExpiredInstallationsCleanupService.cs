using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class ExpiredInstallationsCleanupService : BackgroundService
{
    readonly PurgeExpiredRegistrations                   _purge;
    readonly ILogger<ExpiredInstallationsCleanupService> _logger;
    readonly TimeSpan                                    _duration;

    public ExpiredInstallationsCleanupService(PurgeExpiredRegistrations purge, CleanUpSettings settings,
                                              ILogger<ExpiredInstallationsCleanupService> logger)
        : this(purge, logger, settings.TimerDuration) {}

    public ExpiredInstallationsCleanupService(PurgeExpiredRegistrations purge,
                                              ILogger<ExpiredInstallationsCleanupService> logger, TimeSpan duration)
    {
        _purge    = purge;
        _logger   = logger;
        _duration = duration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_duration);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Starting daily expired installations cleanup...");

                await _purge.Off(stoppingToken);

                _logger.LogInformation("Daily cleanup finished");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in cleanup background service");
            }

            await timer.WaitForNextTickAsync(stoppingToken).Off();
        }
    }
}