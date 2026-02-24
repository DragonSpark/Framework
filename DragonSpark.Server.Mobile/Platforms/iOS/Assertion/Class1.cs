using System;
using System.Buffers;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Objects;
using DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using PeterO.Cbor;
using Array = System.Array;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

public readonly record struct AssertionRequest(string Key, Array<byte> Challenge, Array<byte> Payload);

public readonly record struct AssertionCounterInput(AssertionRequest Request, IAttestationRecord Attestation);

public readonly record struct VerifyPublicKeyInput(string Id, Array<byte> Challenge, Array<byte> Hash, Array<byte> Key);

sealed class VerifyPublicKey : IArray<VerifyPublicKeyInput, byte>
{
    public static VerifyPublicKey Default { get; } = new();

    VerifyPublicKey() {}

    public Array<byte> Get(VerifyPublicKeyInput parameter)
    {
        var (id, challenge, hash, key) = parameter;
        using var sha = SHA256.Create();
        if (sha.ComputeHash(key).SequenceEqual(hash))
        {
            var compute = sha.ComputeHash(challenge);
            var keyId   = Convert.FromBase64String(id);
            return sha.ComputeHash(compute.Concat(keyId).ToArray());
        }

        return Array<byte>.Empty;
    }
}

public sealed class AssertionCounter : ISelect<AssertionCounterInput, uint?>
{
    public static AssertionCounter Default { get; } = new();

    AssertionCounter() : this(VerifyPublicKey.Default, GetAssertionPayload.Default, DetermineCounter.Default) {}

    readonly IArray<VerifyPublicKeyInput, byte>    _expected;
    readonly IArray<AssertionPayloadInput, byte>   _payload;
    readonly ISelect<DetermineCounterInput, uint?> _counter;

    public AssertionCounter(IArray<VerifyPublicKeyInput, byte> expected, IArray<AssertionPayloadInput, byte> payload,
                            ISelect<DetermineCounterInput, uint?> counter)
    {
        _expected = expected;
        _payload  = payload;
        _counter  = counter;
    }

    public uint? Get(AssertionCounterInput parameter)
    {
        var ((key, challenge, payload), record) = parameter;
        var expected = _expected.Get(new(key, challenge, record.PublicKeyHash, record.PublicKey));
        if (expected.Length > 0)
        {
            var bytes  = _payload.Get(new(payload, record.PublicKey, SHA256.HashData(challenge)));
            var result = _counter.Get(new(bytes, record.Receipt, expected));
            if (result > record.Count)
            {
                return result;
            }
        }

        return null;
    }
}

public readonly record struct DetermineCounterInput(Array<byte> Input, Array<byte> Receipt, Array<byte> Expected);

sealed class DetermineCounter : ISelect<DetermineCounterInput, uint?>
{
    public static DetermineCounter Default { get; } = new();

    DetermineCounter() : this(Materialize<AuthenticatorData>.Default) {}

    readonly IMaterialize<AuthenticatorData> _instance;
    readonly byte                            _length;

    public DetermineCounter(IMaterialize<AuthenticatorData> instance, byte length = 37)
    {
        _instance = instance;
        _length   = length;
    }

    public uint? Get(DetermineCounterInput parameter)
    {
        var (input, receipt, expected) = parameter;
        if (input.Length >= _length)
        {
            var       instance = _instance.Get(input);
            using var lease    = input.AsValueEnumerable().Skip(_length).ToArray(ArrayPool<byte>.Shared);
            if (lease.SequenceEqual(receipt.Open()) && instance.Nonce.SequenceEqual(expected))
            {
                var result = instance.Counter;
                if (BitConverter.IsLittleEndian)
                {
                    var bytes = BitConverter.GetBytes(result);
                    Array.Reverse(bytes);
                    return BitConverter.ToUInt32(bytes, 0);
                }

                return result;
            }
        }

        return null;
    }
}

// ISSUE: Assertions is sort of a nebulous obscurity at the moment: https://github.com/dotnet/maui/discussions/31169
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
            CBORObject cbor;
            try
            {
                cbor = CBORObject.DecodeFromBytes(source);
            }
            catch
            {
                return Array<byte>.Empty;
            }

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

                        // Load public key (secp256r1 / P-256)
                        // Assumes uncompressed or compressed point bytes
                        var curve     = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256r1");
                        var domain    = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
                        var q         = curve.Curve.DecodePoint(publicKeyBytes);
                        var publicKey = new ECPublicKeyParameters(q, domain);

                        // Parse DER signature to r/s
                        Asn1Sequence sigSeq;
                        try
                        {
                            sigSeq = (Asn1Sequence)Asn1Object.FromByteArray(signature);
                        }
                        catch
                        {
                            return Array<byte>.Empty;
                        }

                        if (sigSeq.Count == 2)
                        {
                            // ... after loading publicKey and computing nonce

// Verify using NONEwithECDSA (takes pre-hashed digest + full DER sig)
                            // Verify using full DER signature (no manual r/s needed)
                            var signer = SignerUtilities.GetSigner("SHA256withECDSA");
                            signer.Init(false, publicKey);
                            signer.BlockUpdate(nonce, 0, nonce.Length);

                            var verified = signer.VerifySignature(signature);  // signature is the full DER bytes

                            return verified ? authenticatorData : Array<byte>.Empty;
                        }
                    }
                }
            }
        }

        return Array<byte>.Empty; // Or your empty equivalent
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AuthenticatorData
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] Nonce;                               // 32-byte SHA256 hash
    public                               byte Flags;   // 1-byte flags
    [MarshalAs(UnmanagedType.U4)] public uint Counter; // 4-byte counter (big-endian)
}