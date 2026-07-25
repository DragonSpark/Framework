using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

public class ExistingAwareAttest<T> : IStopAware<T>
{
    readonly IExistingAttestation               _existing;
    readonly IStopAware<T>                      _previous;
    readonly Func<ExistingAttestationResult, T> _select;

    protected ExistingAwareAttest(IExistingAttestation existing, IStopAware<T> previous,
                                  Func<ExistingAttestationResult, T> select)
    {
        _existing = existing;
        _previous = previous;
        _select   = select;
    }

    public async ValueTask<T> Get(CancellationToken parameter)
    {
        var existing = await _existing.Off(parameter);
        var result   = existing is not null ? _select(existing) : await _previous.Off(parameter);
        return result;
    }
}