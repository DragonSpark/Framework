using Android.Security.Keystore;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Java.Security;
using Java.Security.Spec;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class GenerateKeyPair : IResult<KeyPair>
{
    public static GenerateKeyPair Default { get; } = new();

    GenerateKeyPair() : this(StoreAlias.Default, KeyStoreName.Default, new ECGenParameterSpec("secp256r1")) {}

    readonly string                  _alias, _key;
    readonly IAlgorithmParameterSpec _parameter;

    public GenerateKeyPair(string alias, string key, IAlgorithmParameterSpec parameter)
    {
        _alias     = alias;
        _key       = key;
        _parameter = parameter;
    }

    public KeyPair Get()
    {
        var generator = KeyPairGenerator.GetInstance(KeyProperties.KeyAlgorithmEc, _key).Verify();

        var parameter = new KeyGenParameterSpec.Builder(_alias, KeyStorePurpose.Sign)
                        .SetAlgorithmParameterSpec(_parameter)
                        .Verify()
                        .SetDigests(KeyProperties.DigestSha256, KeyProperties.DigestNone)
                        .SetUserAuthenticationRequired(false)
                        .SetKeySize(256)
                        .Build();

        generator.Initialize(parameter);
        return generator.GenerateKeyPair().Verify();
    }
}