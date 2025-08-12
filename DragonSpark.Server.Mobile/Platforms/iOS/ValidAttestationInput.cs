namespace DragonSpark.Server.Mobile.Platforms.iOS;

public readonly record struct ValidAttestationInput(
    Attestation Attestation,
    string Challenge,
    string BundleId,
    string KeyHash,
    string Format = "apple-appattest");