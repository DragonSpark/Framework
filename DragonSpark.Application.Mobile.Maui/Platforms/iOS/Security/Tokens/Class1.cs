using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

class Class1;

// --------------- Platform placeholders (implement with your attested key) ---------------
public sealed class DeviceKeyProvider : IDeviceKeyProvider
{
    public ValueTask<PublicJWK> Get(CancellationToken parameter)
    {
        // iOS: extract x/y from SecKey public; Android: from ECPublicKey ECPoint
        // Compute JKT = RFC7638 thumbprint of {"kty":"EC","crv":"P-256","x":"...","y":"..."}
        throw new NotImplementedException("Implement using your hardware key.");
    }
}

public sealed class DeviceSigner : IDeviceSigner
{
    public ValueTask<ReadOnlyMemory<byte>> Get(Stop<ReadOnlyMemory<byte>> parameter)
    {
        // iOS: SecKey.CreateSignature(SecKeyAlgorithm.EcdsaSignatureDigestX962Sha256)
        // Android: Signature.getInstance("NONEwithECDSA") with AndroidKeyStore private key
        throw new NotImplementedException("Implement using your hardware private key.");
    }
}