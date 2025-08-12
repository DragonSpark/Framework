using System;
using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

sealed class PayloadHash : IArray<PayloadHashInput, byte>
{
    public static PayloadHash Default { get; } = new();

    PayloadHash() : this(HashedText.Default, NewLeasing<byte>.Default) {}

    readonly INewLeasing<byte>       _new;
    readonly ISelect<string, byte[]> _hash;

    public PayloadHash(ISelect<string, byte[]> hash, INewLeasing<byte> @new)
    {
        _new  = @new;
        _hash = hash;
    }

    public Array<byte> Get(PayloadHashInput parameter)
    {
        var (data, challenge) = parameter;

        var       hash    = _hash.Get(challenge);
        using var leasing = _new.Get(data.Length + hash.Length.Grade());
        var       nonce   = leasing.Store;
        Buffer.BlockCopy(data, 0, nonce, 0, data.Length.Degrade());
        Buffer.BlockCopy(hash, 0, nonce, data.Length.Degrade(), hash.Length);
        var span   = leasing.AsSpan();
        var result = SHA256.HashData(span);
        return result;
    }
}