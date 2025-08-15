namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public readonly record struct AttestationInstanceInput(
    Attestation Attestation,
    string Challenge,
    string BundleId,
    string KeyHash,
    string Environment = "appattestdevelop",
    string Format = "apple-appattest");