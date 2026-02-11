namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

public sealed record ApplicationIntegrity(bool IsTrusted, string Verdict)
{
    public ApplicationIntegrity(string Verdict) : this(Verdict == "PLAY_RECOGNIZED", Verdict) {}
}