using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Attestation;

sealed class ExistingAttestation : IExistingAttestation
{
    readonly IValidationIdentity _identity;
    readonly IChallenge          _challenge;
    readonly IAssertionToken     _assertion;

    public ExistingAttestation(IValidationIdentity identity, IChallenge challenge, IAssertionToken assertion)
    {
        _identity  = identity;
        _challenge = challenge;
        _assertion = assertion;
    }

    public async ValueTask<ExistingAttestationResult?> Get(CancellationToken parameter)
    {
        var identity = await _identity.Off(parameter);

        if (identity is not null)
        {
            var (subject, hash) = identity;
            var challenge = await _challenge.Off(parameter);
            var assertion = await _assertion.Off(new(challenge.Challenge, parameter));
            return new(subject, assertion, hash, challenge);
        }

        return null;
    }
}