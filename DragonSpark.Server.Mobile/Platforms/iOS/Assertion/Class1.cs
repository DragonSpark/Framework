using System;
using System.Buffers;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Objects;
using DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto.Parameters;
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

    AssertionCounter() : this(VerifyPublicKey.Default, GetAttestationPayload.Default, DetermineCounter.Default) {}

    readonly IArray<VerifyPublicKeyInput, byte>    _expected;
    readonly IArray<AttestationPayloadInput, byte> _payload;
    readonly ISelect<DetermineCounterInput, uint?> _counter;

    public AssertionCounter(IArray<VerifyPublicKeyInput, byte> expected, IArray<AttestationPayloadInput, byte> payload,
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
            var bytes  = _payload.Get(new(payload, record.PublicKey));
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

// TODO: Assertions
public readonly record struct AttestationPayloadInput(Array<byte> Source, Array<byte> PublicKey);

sealed class GetAttestationPayload : IArray<AttestationPayloadInput, byte>
{
    public static GetAttestationPayload Default { get; } = new();

    GetAttestationPayload() {}

    public Array<byte> Get(AttestationPayloadInput parameter)
    {
        var (source, key) = parameter;
        var curve     = SecNamedCurves.GetByName("secp256r1");
        var domain    = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
        var publicKey = new ECPublicKeyParameters(curve.Curve.DecodePoint(key), domain);
        var signed    = new CmsSignedData(source);
        var signer    = signed.GetSignerInfos().GetSigners().Only();
        return (signer?.Verify(publicKey) ?? false) && signed.SignedContent is CmsProcessableByteArray x
                   ? x.GetByteArray()
                   : Array<byte>.Empty;
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