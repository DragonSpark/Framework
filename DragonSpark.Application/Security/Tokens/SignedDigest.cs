using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Security.Tokens;

sealed class SignedDigest : IStopAware<string, ReadOnlyMemory<byte>>
{
    readonly IArray<string, byte> _hash;
    readonly IDeviceSigner        _signer;

    public SignedDigest(IDeviceSigner signer) : this(Hash.Default, signer) {}

    public SignedDigest(IArray<string, byte> hash, IDeviceSigner signer)
    {
        _hash   = hash;
        _signer = signer;
    }

    public async ValueTask<ReadOnlyMemory<byte>> Get(Stop<string> parameter)
    {
        var digest = _hash.Get(parameter).Open();
        var result = await _signer.Off(new(digest, parameter));
        return result;
    }
}