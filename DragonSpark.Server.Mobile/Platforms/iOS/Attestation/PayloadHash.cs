using System.Security.Cryptography;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

sealed class PayloadHash : IArray<PayloadHashInput, byte>
{
    public static PayloadHash Default { get; } = new();

    PayloadHash() : this(HashedBase64UrlData.Default, NewLeasing<byte>.Default) {}

    readonly IParser<byte[]>   _hash;
    readonly INewLeasing<byte> _new;

    public PayloadHash(IParser<byte[]> hash, INewLeasing<byte> @new)
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
        var result = SHA256.HashData(leasing.AsSpan());
        return result;
    }
}