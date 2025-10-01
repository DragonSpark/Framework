using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

public class Attest<T> : StopAware<T>
{
    protected Attest(IAttest previous, Func<NewAttestationResult, T> select)
        : base(previous.Then().Select(x => x.To<NewAttestationResult>()).Select(select)) {}
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
        return new NewAttestationResult(key, challenge, attestation);
    }
}