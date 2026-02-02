using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

public interface IAttestationIdentity : IStopAware<AttestationIdentityView?>;

// TODO
public sealed record AttestationIdentityView(Guid Identity, string KeyHash);

public interface IExistingAttestation : IStopAware<ExistingAttestationResult?>;

sealed class ExistingAttestation : IExistingAttestation
{
    readonly IAttestationIdentity _identity;
    readonly IChallenge           _challenge;

    public ExistingAttestation(IAttestationIdentity identity, IChallenge challenge)
    {
        _identity  = identity;
        _challenge = challenge;
    }

    public async ValueTask<ExistingAttestationResult?> Get(CancellationToken parameter)
    {
        var identity = await _identity.Off(parameter);

        if (identity is not null)
        {
            var (subject, hash) = identity;
            var challenge = await _challenge.Off(parameter);
            return new(subject, hash, challenge);
        }

        return null;
    }
}