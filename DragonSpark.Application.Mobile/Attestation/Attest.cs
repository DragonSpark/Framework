using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

public class Attest<T> : StopAware<T>
{
    protected Attest(IAttest previous, Func<AttestationResult, T> select) : base(previous.Then().Select(select)) {}
}

sealed class Attest : IAttest
{
    readonly IStopAware<string> _key;
    readonly IChallenge         _challenge;
    readonly IAttestationToken  _token;

    public Attest(ClientHash key, IChallenge challenge, IAttestationToken token)
    {
        _key       = key;
        _challenge = challenge;
        _token     = token;
    }

    public async ValueTask<AttestationResult> Get(CancellationToken parameter)
    {
        var key         = await _key.Off(parameter);
        var challenge   = await _challenge.Off(parameter);
        var attestation = await _token.Off(new(challenge, parameter));
        return new(null, key, challenge, attestation);
    }
}

// TODO

public class PreviousAttestationAwareAttest : IAttest
{
    readonly IAttestationIdentity _identity;
    readonly IAttest              _previous;
    readonly ClientHash           _key;

    protected PreviousAttestationAwareAttest(IAttestationIdentity identity, IAttest previous, ClientHash key)
    {
        _identity = identity;
        _previous = previous;
        _key      = key;
    }

    public async ValueTask<AttestationResult> Get(CancellationToken parameter)
    {
        var identity = await _identity.Off(parameter);
        return identity is not null
                   ? new(identity, await _key.Off(parameter), string.Empty, string.Empty)
                   : await _previous.Off(parameter);
    }
}

public interface IAttestationIdentity : IStopAware<Guid?>;