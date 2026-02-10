using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Tokens;

public sealed class ComputeJkt : IFormatter<Points>
{
    public static ComputeJkt Default { get; } = new();

    ComputeJkt() : this(EncodedHashedText.Default, TokenDataFormatter.Default) {}

    readonly ISelect<string, byte[]> _encoded;
    readonly IFormatter<Array<byte>> _format;

    public ComputeJkt(ISelect<string, byte[]> encoded, IFormatter<Array<byte>> format)
    {
        _encoded = encoded;
        _format  = format;
    }

    public string Get(Points parameter)
    {
        var (x, y) = parameter;
        var json   = $$"""{"crv":"P-256","kty":"EC","x":"{{x}}","y":"{{y}}"}""";
        var hash   = _encoded.Get(json);
        var result = _format.Get(hash);
        return result;
    }
}
