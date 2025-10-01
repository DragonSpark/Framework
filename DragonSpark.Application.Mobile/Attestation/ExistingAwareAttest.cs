using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

public class ExistingAwareAttest<T> : IStopAware<T>
{
    readonly IAttestationIdentity               _identity;
    readonly IStopAware<T>                      _previous;
    readonly Func<ExistingAttestationResult, T> _select;

    protected ExistingAwareAttest(IAttestationIdentity identity, IStopAware<T> previous,
                                  Func<ExistingAttestationResult, T> select)
    {
        _identity = identity;
        _previous = previous;
        _select   = select;
    }

    public async ValueTask<T> Get(CancellationToken parameter)
    {
        var existing = await _identity.Off(parameter);
        var result   = existing is not null ? _select(existing) : await _previous.Off(parameter);
        return result;
    }
}