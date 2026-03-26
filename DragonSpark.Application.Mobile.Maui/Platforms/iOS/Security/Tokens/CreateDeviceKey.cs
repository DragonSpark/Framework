using System;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Text;
using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class CreateDeviceKey : IResult<PublicJWK>
{
    public static CreateDeviceKey Default { get; } = new();

    CreateDeviceKey() : this(SecurityKey.Default) {}

    readonly IResult<SecKey>                    _key;
    readonly ILease<ReadOnlyMemory<byte>, char> _encode;
    readonly IFormatter<Points>                 _jkt;

    public CreateDeviceKey(SecurityKey key) : this(key, Base64UrlCharacterEncoder.Default, ComputeJkt.Default) {}

    public CreateDeviceKey(IResult<SecKey> key, ILease<ReadOnlyMemory<byte>, char> encode, IFormatter<Points> jkt)
    {
        _key    = key;
        _encode = encode;
        _jkt    = jkt;
    }

    public PublicJWK Get()
    {
        var data = _key.Get().GetPublicKey().Verify().GetExternalRepresentation(out var error) ??
                   throw new InvalidOperationException($"Unable to export public key: {error}");

        var bytes = data.ToArray();
        if (bytes.Length == 65 && bytes[0] == 0x04)
        {
            using var x      = _encode.Get(bytes.AsMemory(1, 32));
            using var y      = _encode.Get(bytes.AsMemory(33, 32));
            var       points = new Points(new(x.AsSpan()), new(y.AsSpan()));
            var       jkt    = _jkt.Get(points);

            return new("EC", "P-256", points.X, points.Y, jkt);
        }

        throw new InvalidOperationException("Unexpected EC public key format.");
    }
}