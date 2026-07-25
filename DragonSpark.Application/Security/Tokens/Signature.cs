using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Application.Security.Tokens;

sealed class Signature : IStopAware<string, Leasing<char>>
{
    readonly SignedDigest                                 _digest;
    readonly ISelect<ReadOnlyMemory<byte>, Leasing<char>> _signed;
    readonly ILease<ReadOnlyMemory<char>, char>           _formatter;

    public Signature(SignedDigest digest) : this(digest, Signed.Default, MemoryTokenFormatter.Default) {}

    public Signature(SignedDigest digest, ISelect<ReadOnlyMemory<byte>, Leasing<char>> signed,
                     ILease<ReadOnlyMemory<char>, char> formatter)
    {
        _digest    = digest;
        _signed    = signed;
        _formatter = formatter;
    }

    public async ValueTask<Leasing<char>> Get(Stop<string> parameter)
    {
        var       digest = await _digest.Off(parameter);
        using var signed = _signed.Get(digest);
        var       result = _formatter.Get(signed.AsMemory());
        return result;
    }
}