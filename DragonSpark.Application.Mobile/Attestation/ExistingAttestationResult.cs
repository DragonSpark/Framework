using DragonSpark.Contracts.Security;

namespace DragonSpark.Application.Mobile.Attestation;

public sealed record ExistingAttestationResult(
    Guid Identity,
    string Payload,
    string KeyHash,
    ChallengeResponse Challenge)
    : AttestationResult(KeyHash, Challenge);