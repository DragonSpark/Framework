using System;
using DragonSpark.Compose;
using Java.Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class LoadKeyPair : ILoadKeyPair
{
    public static LoadKeyPair Default { get; } = new();

    LoadKeyPair() : this(StoreAlias.Default) {}

    readonly string _alias;

    public LoadKeyPair(string alias) => _alias = alias;

    public KeyPair Get(KeyStore parameter)
    {
        var entry = parameter.GetEntry(_alias, null) as KeyStore.PrivateKeyEntry
                    ?? throw new InvalidOperationException($"Alias '{_alias}' does not contain a private key.");

        return new(entry.Certificate.Verify().PublicKey, entry.PrivateKey);
    }
}