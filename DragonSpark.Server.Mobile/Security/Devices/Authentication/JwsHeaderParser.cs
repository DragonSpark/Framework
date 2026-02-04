using DragonSpark.Application.Security.Tokens;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class JwsHeaderParser : ISelect<HttpRequest, JwsResult?>
{
    public static JwsHeaderParser Default { get; } = new();

    JwsHeaderParser() : this(JwsParser.Default, ProofName.Default) {}

    readonly IParser<JwsResult?> _parser;
    readonly string              _name;

    public JwsHeaderParser(IParser<JwsResult?> parser, string name)
    {
        _parser = parser;
        _name   = name;
    }

    public JwsResult? Get(HttpRequest parameter)
    {
        var header = parameter.Headers[_name].ToString();
        var result = _parser.Get(header);
        return result;
    }
}