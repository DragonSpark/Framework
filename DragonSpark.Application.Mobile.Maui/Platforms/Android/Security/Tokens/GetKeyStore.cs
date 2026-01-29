using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Java.Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class GetKeyStore : IResult<KeyStore>
{
    public static GetKeyStore Default { get; } = new();

    GetKeyStore() : this(KeyStoreName.Default) {}

    readonly string _key;

    public GetKeyStore(string key) => _key = key;

    public KeyStore Get()
    {
        var result = KeyStore.GetInstance(_key).Verify();
        result.Load(null);
        return result;
    }
}