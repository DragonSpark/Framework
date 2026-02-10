using System;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Foundation;
using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class DeviceSigner : IDeviceSigner
{
    public static DeviceSigner Default { get; } = new();

    DeviceSigner() : this(SecurityKey.Default) {}

    readonly SecKey _key;

    public DeviceSigner(SecKey key) => _key = key;

    public ValueTask<ReadOnlyMemory<byte>> Get(Stop<ReadOnlyMemory<byte>> parameter)
    {
        var (digest, _) = parameter;
        var data = NSData.FromArray(digest.ToArray());
        var signature = _key.CreateSignature(SecKeyAlgorithm.EcdsaSignatureDigestX962Sha256, data, out var error) ??
                        throw new InvalidOperationException($"Unable to sign digest: {error}");
        return signature.ToArray().AsMemory().AsReadOnly().ToOperation();
    }
}