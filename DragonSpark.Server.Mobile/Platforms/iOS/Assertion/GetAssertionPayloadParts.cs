using System.Buffers;
using System.Security.Cryptography;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using NetFabric.Hyperlinq;
using PeterO.Cbor;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

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
                using var sha = SHA256.Create();
                using var combined = result.Concat(_hash.Get(challenge).Open())
                                           .AsValueEnumerable()
                                           .ToArray(ArrayPool<byte>.Shared);
                return new(sha.ComputeHash(combined.Rented, 0, combined.Length), signature.GetByteString(), result);
            }
        }

        return null;
    }
}