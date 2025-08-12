namespace DragonSpark.Server.Mobile.Platforms.iOS;

public readonly record struct ValidatedAttestationInput(
    string Challenge,
    string BundleId,
    string KeyHash,
    string Attestation,
    string Format = "apple-appattest");