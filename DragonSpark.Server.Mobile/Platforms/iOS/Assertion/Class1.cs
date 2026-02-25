using System;
using System.Buffers.Binary;
using System.Linq;
using System.Security.Cryptography;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using PeterO.Cbor;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

public readonly record struct AssertionRequest(string Challenge, Array<byte> Payload);

public readonly record struct AssertionCounterInput(AssertionRequest Request, IAttestationRecord Attestation)
{
    public AssertionCounterInput(string Challenge, string Payload, IAttestationRecord Attestation)
        : this(new(Challenge, Convert.FromBase64String(Payload)), Attestation) {}
}

public readonly record struct VerifyPublicKeyInput(Array<byte> Hash, Array<byte> Key);

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

public sealed class AssertionCounter : ISelect<AssertionCounterInput, uint?>
{
    public static AssertionCounter Default { get; } = new();

    AssertionCounter() : this(VerifyPublicKey.Default, GetAssertionPayload.Default, DetermineCount.Default) {}

    readonly ICondition<VerifyPublicKeyInput>    _expected;
    readonly IArray<AssertionPayloadInput, byte> _payload;
    readonly ISelect<Array<byte>, uint?>         _count;

    public AssertionCounter(ICondition<VerifyPublicKeyInput> expected, IArray<AssertionPayloadInput, byte> payload,
                            ISelect<Array<byte>, uint?> count)
    {
        _expected = expected;
        _payload  = payload;
        _count    = count;
    }

    public uint? Get(AssertionCounterInput parameter)
    {
        var ((challenge, payload), record) = parameter;
        var hash     = HashedBase64UrlData.Default.Get(challenge); // TODO
        var expected = _expected.Get(new(record.PublicKeyHash, record.PublicKey));
        if (expected)
        {
            var bytes = _payload.Get(new(payload, record.PublicKey, hash));
            if (bytes.Length > 0)
            {
                var result = _count.Get(bytes);
                if (result > record.Count)
                {
                    return result;
                }
            }
        }

        return null;
    }
}

sealed class DetermineCount : ISelect<Array<byte>, uint?>
{
    public static DetermineCount Default { get; } = new();

    DetermineCount() : this(37) {}

    readonly byte _length;

    public DetermineCount(byte length) => _length = length;

    public uint? Get(Array<byte> parameter)
        => parameter.Length == _length
               ? BinaryPrimitives.ReadUInt32BigEndian(parameter.Open().AsSpan(33, 4))
               : null;
}

public readonly record struct AssertionPayloadInput(
    Array<byte> Source,        // The raw assertion bytes (from GenerateAssertionAsync)
    Array<byte> PublicKey,     // The raw public key bytes (from attestation credential)
    Array<byte> ClientDataHash // The exact hash passed to GenerateAssertionAsync (critical for nonce)
);

sealed class GetAssertionPayload : IArray<AssertionPayloadInput, byte>
{
    public static GetAssertionPayload Default { get; } = new();

    GetAssertionPayload() {}

    public Array<byte> Get(AssertionPayloadInput parameter)
    {
        var (source, publicKeyBytes, clientDataHash) = parameter; // source = assertion bytes from iOS

        if (source.Length != 0)
        {
            var cbor = CBORObject.DecodeFromBytes(source);
            if (cbor.Type == CBORType.Map && cbor.Count == 2)
            {
                var sigObj  = cbor["signature"];
                var authObj = cbor["authenticatorData"];

                if (sigObj != null && sigObj.Type == CBORType.ByteString && authObj != null &&
                    authObj.Type == CBORType.ByteString)
                {
                    var signature         = sigObj.GetByteString();
                    var authenticatorData = authObj.GetByteString();

                    if (authenticatorData.Length >= 37) // Adjust to your expected min length
                    {
                        byte[] nonce;
                        using (var sha = SHA256.Create())
                        {
                            var combined = authenticatorData.Concat(clientDataHash.Open()).ToArray();
                            nonce = sha.ComputeHash(combined);
                        }

                        var curve     = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256r1");
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
                                signer.BlockUpdate(nonce, 0, nonce.Length);
                                if (signer.VerifySignature(signature))
                                {
                                    return authenticatorData;
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        return Array<byte>.Empty; // Or your empty equivalent
    }
}

/*[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AuthenticatorData
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] Nonce;                               // 32-byte SHA256 hash
    public                               byte Flags;   // 1-byte flags
    [MarshalAs(UnmanagedType.U4)] public uint Counter; // 4-byte counter (big-endian)
}*/