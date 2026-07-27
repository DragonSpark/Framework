using System.Security.Cryptography;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

sealed class VerifyPublicKey : ICondition<VerifyPublicKeyInput>
{
    public static VerifyPublicKey Default { get; } = new();

    VerifyPublicKey() {}

    public bool Get(VerifyPublicKeyInput parameter)
    {
        var (hash, key) = parameter;
        using var sha = SHA256.Create();
        return sha.ComputeHash(key).SequenceEqual(hash);
    }
}