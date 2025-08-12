using System;
using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

sealed class PayloadHash : IArray<PayloadHashInput, byte>
{
    public static PayloadHash Default { get; } = new();

    PayloadHash() : this(NewLeasing<byte>.Default) {}

    readonly INewLeasing<byte> _new;

    public PayloadHash(INewLeasing<byte> @new)
    {
        _new = @new;
    }

    public Array<byte> Get(PayloadHashInput parameter)
    {
        var (data, challenge) = parameter;

        var       hash    = SHA256.HashData(Convert.FromBase64String(challenge));
        using var leasing = _new.Get(data.Length + hash.Length.Grade());
        var       nonce   = leasing.Store;
        Buffer.BlockCopy(data, 0, nonce, 0, data.Length.Degrade());
        Buffer.BlockCopy(hash, 0, nonce, data.Length.Degrade(), hash.Length);
        var span   = leasing.AsSpan();
        var result = SHA256.HashData(span);
        return result;
    }
}