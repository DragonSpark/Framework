namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public readonly record struct ValidAttestationInput(
    Attestation Attestation,
    string Challenge,
    string BundleId,
    string KeyHash,
    string Environment = "appattest",
    string Format = "apple-appattest");