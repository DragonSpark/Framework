using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

sealed class GetAssertionPayload : IArray<AssertionPayloadInput, byte>
{
    public static GetAssertionPayload Default { get; } = new();

    GetAssertionPayload() : this(GetAssertionPayloadParts.Default) {}

    readonly ISelect<AssertionPayloadInput, AssertionPayloadParts?> _parts;

    public GetAssertionPayload(ISelect<AssertionPayloadInput, AssertionPayloadParts?> parts)
    {
        _parts = parts;
    }

    public Array<byte> Get(AssertionPayloadInput parameter)
    {
        var (source, publicKeyBytes, _) = parameter;

        var parts = source.Length > 0 ? _parts.Get(parameter) : null;
        if (parts is not null)
        {
            var (nonceHash, signature, result) = parts.Value;
            var curve     = SecNamedCurves.GetByName("secp256r1");
            var domain    = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var q         = curve.Curve.DecodePoint(publicKeyBytes);
            var publicKey = new ECPublicKeyParameters(q, domain);
            var sigSeq    = (Asn1Sequence)Asn1Object.FromByteArray(signature);
            switch (sigSeq.Count)
            {
                case 2:
                {
                    var signer = SignerUtilities.GetSigner("SHA256withECDSA");
                    signer.Init(false, publicKey);
                    signer.BlockUpdate(nonceHash, 0, nonceHash.Length.Degrade());
                    if (signer.VerifySignature(signature))
                    {
                        return result;
                    }

                    break;
                }
            }
        }

        return Array<byte>.Empty; // Or your empty equivalent
    }
}