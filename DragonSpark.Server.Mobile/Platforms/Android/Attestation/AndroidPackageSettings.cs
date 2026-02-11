namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

public sealed record AndroidPackageSettings
{
    public required string EncodedKey { get; set; }

    public required string PackageName { get; set; }
}