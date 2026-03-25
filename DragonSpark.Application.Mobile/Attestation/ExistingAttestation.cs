using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Attestation;

sealed class ExistingAttestation : IExistingAttestation
{
    readonly IValidationIdentity _identity;
    readonly IChallenge          _challenge;
    readonly IAssertionToken     _assertion;
    readonly ITokens             _tokens;

    public ExistingAttestation(IValidationIdentity identity, IChallenge challenge, IAssertionToken assertion,
                               ITokens tokens)
    {
        _identity  = identity;
        _challenge = challenge;
        _assertion = assertion;
        _tokens    = tokens;
    }

    public async ValueTask<ExistingAttestationResult?> Get(CancellationToken parameter)
    {
        var identity = await _identity.Off(parameter);

        if (identity is not null)
        {
            var (subject, hash) = identity;
            _tokens.Execute();
            var challenge = await _challenge.Off(parameter);
            var assertion = await _assertion.Off(new(challenge.Challenge, parameter));
            return new(subject, assertion, hash, challenge);
        }

        return null;
    }
}