using System;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

public sealed class DeviceSigner : IDeviceSigner
{
    public ValueTask<ReadOnlyMemory<byte>> Get(Stop<ReadOnlyMemory<byte>> parameter)
    {
        // iOS: SecKey.CreateSignature(SecKeyAlgorithm.EcdsaSignatureDigestX962Sha256)
        // Android: Signature.getInstance("NONEwithECDSA") with AndroidKeyStore private key
        throw new NotImplementedException("Implement using your hardware private key.");
    }
}