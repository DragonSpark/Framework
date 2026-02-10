using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Java.Security;
using Java.Security.Interfaces;
using Java.Security.Spec;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class DeterminePoint : IResult<ECPoint>
{
    public static DeterminePoint Default { get; } = new();

    DeterminePoint() : this(GetKeyStore.Default, GeneratorAwareLoadKeyPair.Default, SpkiPointDecoder.Default) {}

    readonly IResult<KeyStore>        _store;
    readonly ILoadKeyPair             _load;
    readonly ISelect<byte[], ECPoint> _decoder;

    public DeterminePoint(IResult<KeyStore> store, ILoadKeyPair load, ISelect<byte[], ECPoint> decoder)
    {
        _store   = store;
        _load    = load;
        _decoder = decoder;
    }

    public ECPoint Get()
    {
        var store  = _store.Get();
        var pair   = _load.Get(store);
        var pub    = pair.Public.Verify();
        var result = pub is IECPublicKey ec ? ec.GetW().Verify() : _decoder.Get(pub.GetEncoded().Verify());
        return result;
    }
}