using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class NonceCleanupService : BackgroundService
{
    readonly CleanUpNonces                _clean;
    readonly TimeSpan                     _interval;
    readonly ILogger<NonceCleanupService> _logger;

    public NonceCleanupService(CleanUpNonces clean, ILogger<NonceCleanupService> logger)
        : this(clean, TimeSpan.FromMinutes(10), logger) {}

    public NonceCleanupService(CleanUpNonces clean, TimeSpan interval, ILogger<NonceCleanupService> logger)
    {
        _clean    = clean;
        _interval = interval;
        _logger   = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_interval);
        while (await timer.WaitForNextTickAsync(stoppingToken).Off())
        {
            try
            {
                var deleted = await _clean.Off(stoppingToken);
                if (deleted > 0)
                {
                    _logger.LogDebug("Nonce cleanup removed {Count} rows", deleted);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Nonce cleanup failed");
            }
        }
    }
}