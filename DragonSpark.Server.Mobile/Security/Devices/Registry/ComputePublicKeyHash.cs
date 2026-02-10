using System;
using System.Security.Cryptography;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class ComputePublicKeyHash : ILease<Points, byte>
{
    public static ComputePublicKeyHash Default { get; } = new();

    ComputePublicKeyHash()
        : this(Cryptography.Base64UrlDecode.Default, NewLeasing<byte>.Default, Base64UrlMemoryEncoder.Default) {}

    readonly ILease<ReadOnlyMemory<char>, byte> _decode;
    readonly INewLeasing<byte>                  _new;
    readonly ILease<ReadOnlyMemory<byte>, byte> _encode;

    public ComputePublicKeyHash(ILease<ReadOnlyMemory<char>, byte> decode, INewLeasing<byte> @new,
                                ILease<ReadOnlyMemory<byte>, byte> encode)
    {
        _decode = decode;
        _new    = @new;
        _encode = encode;
    }

    public Leasing<byte> Get(Points parameter)
    {
        var (xB64, yB64) = parameter;
        using var x  = _decode.Get(xB64.AsMemory());
        using var y  = _decode.Get(yB64.AsMemory());
        using var to = _new.Get(1 + x.Length + y.Length);

        to.Store[0] = 0x04;
        Buffer.BlockCopy(x.Store, 0, to.Store, 1, (int)x.Length);
        Buffer.BlockCopy(y.Store, 0, to.Store, (int)(1 + x.Length), (int)y.Length);

        using var sha    = SHA256.Create();
        var       hash   = sha.ComputeHash(to.Store, 0, (int)to.Length);
        var       result = _encode.Get(hash);
        return result;
    }
}