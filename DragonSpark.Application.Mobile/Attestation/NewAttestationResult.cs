using DragonSpark.Contracts.Security;

namespace DragonSpark.Application.Mobile.Attestation;

public sealed record NewAttestationResult(string KeyHash, ChallengeResponse Challenge, string Attestation)
    : AttestationResult(KeyHash, Challenge);