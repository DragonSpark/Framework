using DragonSpark.Compose;
using Java.Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class LoadKeyPair : ILoadKeyPair
{
    public static LoadKeyPair Default { get; } = new();

    LoadKeyPair() : this("dpop-device-key") {}

    readonly string _alias;

    public LoadKeyPair(string alias)
    {
        _alias = alias;
    }

    public KeyPair Get(KeyStore parameter)
    {
        return new KeyPair(parameter.GetCertificate(_alias).Verify().PublicKey,
                           parameter.GetKey(_alias, null)?.To<IPrivateKey>());
    }
}