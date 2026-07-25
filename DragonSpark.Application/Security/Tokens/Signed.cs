using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Application.Security.Tokens;

sealed class Signed : ISelect<ReadOnlyMemory<byte>, Leasing<char>>
{
    public static Signed Default { get; } = new();

    Signed() : this(DerToJose.Default, Base64UrlCharacterEncoder.Default) {}

    readonly ILease<ReadOnlyMemory<byte>, byte> _jose;
    readonly ILease<ReadOnlyMemory<byte>, char> _encode;

    public Signed(ILease<ReadOnlyMemory<byte>, byte> jose, ILease<ReadOnlyMemory<byte>, char> encode)
    {
        _jose   = jose;
        _encode = encode;
    }

    public Leasing<char> Get(ReadOnlyMemory<byte> parameter)
    {
        using var first = _jose.Get(parameter);
        return _encode.Get(first.AsMemory());
    }
}