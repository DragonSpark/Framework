using DragonSpark.Compose;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class NonceCleanupOperation : DragonSpark.Model.Operations.Results.Stop.IStopAware<bool>
{
    readonly CleanUpNonces                  _clean;
    readonly ILogger<NonceCleanupOperation> _logger;

    public NonceCleanupOperation(CleanUpNonces clean, ILogger<NonceCleanupOperation> logger)
    {
        _clean  = clean;
        _logger = logger;
    }

    public async ValueTask<bool> Get(CancellationToken parameter)
    {
        try
        {
            var deleted = await _clean.Off(parameter);
            if (deleted > 0)
            {
                _logger.LogDebug("Nonce cleanup removed {Count} rows", deleted);
            }
        }
        catch (OperationCanceledException) when (parameter.IsCancellationRequested)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Nonce cleanup failed");
        }

        return true;
    }
}