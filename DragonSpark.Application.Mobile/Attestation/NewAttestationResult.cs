namespace DragonSpark.Application.Mobile.Attestation;

public sealed record NewAttestationResult(string KeyHash, string Challenge, string Attestation)
    : AttestationResult(KeyHash);