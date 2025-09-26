using DragonSpark.Composition.Compose;
using DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

namespace DragonSpark.Server.Mobile.Platforms.Android;

public static class Extensions
{
    public static BuildHostContext WithVerification<T>(this BuildHostContext @this) where T : class, IVerificationRecord
        => @this.Configure(Attestation.Registrations<T>.Default);
}