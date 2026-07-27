using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Foundation;
using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class DeviceSigner : IDeviceSigner
{
    public static DeviceSigner Default { get; } = new();

    DeviceSigner() : this(SecurityKey.Default) {}

    readonly IResult<SecKey> _key;

    public DeviceSigner(IResult<SecKey> key) => _key = key;

    public ValueTask<ReadOnlyMemory<byte>> Get(Stop<ReadOnlyMemory<byte>> parameter)
    {
        var (digest, _) = parameter;
        var data = NSData.FromArray(digest.ToArray());
        var key  = _key.Get();
        var signature = key.CreateSignature(SecKeyAlgorithm.EcdsaSignatureDigestX962Sha256, data, out var error) ??
                        throw new InvalidOperationException($"Unable to sign digest: {error}");
        return signature.ToArray().AsMemory().AsReadOnly().ToOperation();
    }
}