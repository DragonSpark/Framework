using DragonSpark.Model.Sequences;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Tokens;

public sealed class TokenDataFormatter : IFormatter<Array<byte>>
{
    public static TokenDataFormatter Default { get; } = new();

    TokenDataFormatter() : this(Convert.ToBase64String, TokenFormatter.Default) {}

    readonly Func<byte[], string> _convert;
    readonly IFormatter<string>   _previous;

    public TokenDataFormatter(Func<byte[], string> convert, IFormatter<string> previous)
    {
        _convert  = convert;
        _previous = previous;
    }

    public string Get(Array<byte> parameter) => _previous.Get(_convert(parameter));
}