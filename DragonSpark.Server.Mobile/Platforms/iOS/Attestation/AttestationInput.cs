namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public readonly record struct AttestationInput(
    string Attestation,
    string Challenge,
    string BundleId,
    string KeyHash,
    string Environment = "appattestdevelop",
    string Format = "apple-appattest");