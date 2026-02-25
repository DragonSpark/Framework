using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Linq;
using System.Security.Cryptography;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;
using DragonSpark.Text;
using NetFabric.Hyperlinq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
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
        var expected = _expected.Get(new(record.PublicKeyHash, record.PublicKey));
        if (expected)
        {
            var bytes = _payload.Get(new(payload, record.PublicKey, challenge));
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

    DetermineCount() : this(AuthenticationDataLength.Default) {}

    readonly byte _length;

    public DetermineCount(byte length)
    {
        _length = length;
    }

    public uint? Get(Array<byte> parameter)
    {
        return parameter.Length == _length
                   ? BinaryPrimitives.ReadUInt32BigEndian(parameter.Open().AsSpan(33, 4))
                   : null;
    }
}

sealed class AuthenticationDataLength : Instance<byte>
{
    public static AuthenticationDataLength Default { get; } = new();

    AuthenticationDataLength() : base(37) {}
}

public readonly record struct AssertionPayloadInput(
    Array<byte> Source,
    Array<byte> PublicKey,
    string ClientDataHash);

public readonly record struct AssertionPayloadParts(
    Array<byte> NonceHash,
    Array<byte> Signature,
    Array<byte> Authentication);

sealed class GetAssertionPayloadParts : ISelect<AssertionPayloadInput, AssertionPayloadParts?>
{
    public static GetAssertionPayloadParts Default { get; } = new();

    GetAssertionPayloadParts() : this(HashedBase64UrlData.Default, AuthenticationDataLength.Default) {}

    readonly IParser<byte[]> _hash;
    readonly byte            _length;

    public GetAssertionPayloadParts(IParser<byte[]> hash, byte length)
    {
        _hash   = hash;
        _length = length;
    }

    public AssertionPayloadParts? Get(AssertionPayloadInput parameter)
    {
        var (source, _, challenge) = parameter;
        var instance = CBORObject.DecodeFromBytes(source);
        if (instance is { Type: CBORType.Map, Count: 2 })
        {
            var signature      = instance["signature"];
            var authentication = instance["authenticatorData"];

            var result = authentication.GetByteString();
            if (result.Length >= _length &&
                signature is { Type: CBORType.ByteString } && authentication is { Type: CBORType.ByteString })
            {
                using var sha  = SHA256.Create();
                using var combined = result.Concat(_hash.Get(challenge).Open())
                                           .AsValueEnumerable()
                                           .ToArray(ArrayPool<byte>.Shared);
                return new(sha.ComputeHash(combined.Rented, 0, combined.Length), signature.GetByteString(), result);
            }
        }

        return null;
    }
}

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