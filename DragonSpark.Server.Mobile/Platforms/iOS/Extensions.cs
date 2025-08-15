using DragonSpark.Composition.Compose;
using DragonSpark.Server.Mobile.Platforms.iOS.Attestation;
using DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public static class Extensions
{
    public static BuildHostContext WithAttestationRecord<T>(this BuildHostContext @this)
        where T : class, IAttestationRecord
        => @this.Configure(Registrations<T>.Default);
}