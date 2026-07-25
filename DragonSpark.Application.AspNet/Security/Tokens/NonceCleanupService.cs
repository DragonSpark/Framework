using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class NonceCleanupService : BackgroundService
{
    readonly NonceCleanupOperation         _first;
    readonly PeriodicNonceCleanupOperation _periodic;

    public NonceCleanupService(NonceCleanupOperation first, PeriodicNonceCleanupOperation periodic)
    {
        _first    = first;
        _periodic = periodic;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (await _first.Off(stoppingToken))
        {
            await _periodic.Off(stoppingToken);
        }
    }
}