using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class PeriodicNonceCleanupOperation : IStopAware
{
    readonly NonceCleanupOperation _operation;
    readonly TimeSpan              _interval;

    public PeriodicNonceCleanupOperation(NonceCleanupOperation operation) : this(operation, TimeSpan.FromMinutes(10)) {}

    public PeriodicNonceCleanupOperation(NonceCleanupOperation operation, TimeSpan interval)
    {
        _operation = operation;
        _interval  = interval;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        using var timer = new PeriodicTimer(_interval);
        while (await timer.WaitForNextTickAsync(parameter).Off())
        {
            if (!await _operation.Off(parameter))
            {
                break;
            }
        }
    }
}