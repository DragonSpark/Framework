using DragonSpark.Composition.Compose;
using DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public static class Extensions
{
    public static BuildHostContext WithAttestationRecord<T>(this BuildHostContext @this)
        where T : class, IAttestationRecord
        => @this.Configure(Registrations<T>.Default);
}