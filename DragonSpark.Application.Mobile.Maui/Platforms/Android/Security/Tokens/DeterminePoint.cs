using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Java.Security;
using Java.Security.Interfaces;
using Java.Security.Spec;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class DeterminePoint : IResult<ECPoint>
{
    public static DeterminePoint Default { get; } = new();

    DeterminePoint() : this(GetKeyStore.Default, GeneratorAwareLoadKeyPair.Default) {}

    readonly IResult<KeyStore> _store;
    readonly ILoadKeyPair      _load;

    public DeterminePoint(IResult<KeyStore> store, ILoadKeyPair load)
    {
        _store = store;
        _load  = load;
    }

    public ECPoint Get()
    {
        var store = _store.Get();
        var pair  = _load.Get(store);
        return pair.Public.Verify().To<IECPublicKey>().GetW().Verify();
    }
}