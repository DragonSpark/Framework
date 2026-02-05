using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class JwsHeaderParser : ISelect<HttpRequest, ParsedJws?>
{
    public static JwsHeaderParser Default { get; } = new();

    JwsHeaderParser() : this(JwsParser.Default, ProofHeader.Default) {}

    readonly IParser<ParsedJws?> _parser;
    readonly IHeader             _header;

    public JwsHeaderParser(IParser<ParsedJws?> parser, IHeader header)
    {
        _parser = parser;
        _header = header;
    }

    public ParsedJws? Get(HttpRequest parameter)
    {
        var header = _header.Get(parameter.Headers).Verify();
        var result = _parser.Get(header);
        return result;
    }
}