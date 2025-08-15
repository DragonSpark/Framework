namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

public sealed class AttestationConfiguration
{
    public required string BundleIdentifier { get; set; }

    public string Environment { get; set; } = "appattestdevelop";
}