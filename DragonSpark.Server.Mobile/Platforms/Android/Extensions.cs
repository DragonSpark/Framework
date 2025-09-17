using DragonSpark.Composition.Compose;

namespace DragonSpark.Server.Mobile.Platforms.Android;

public static class Extensions
{
    public static BuildHostContext WithVerification(this BuildHostContext @this)
        => @this.Configure(Attestation.Registrations.Default);
}