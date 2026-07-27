namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

public sealed record DeviceIntegrity(bool IsTrusted, IList<string> Verdict)
{
    public DeviceIntegrity(IList<string> Verdict) : this(Verdict.Contains("MEETS_DEVICE_INTEGRITY"), Verdict) {}
}