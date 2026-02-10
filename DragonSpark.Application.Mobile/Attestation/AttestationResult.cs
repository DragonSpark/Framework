using DragonSpark.Contracts.Security;

namespace DragonSpark.Application.Mobile.Attestation;

public abstract record AttestationResult(string KeyHash, ChallengeResponse Challenge);