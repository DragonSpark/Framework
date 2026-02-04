using System.Text.Json;
using DragonSpark.Application.AspNet.Security;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Model.Selection;
using DragonSpark.Server.Mobile.Security.Devices.Authentication;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class ComposeRequestJwk : IParser<JwkHeader?>
{
    readonly ICurrentContext                  _context;
    readonly ISelect<HttpRequest, JwsResult?> _parser;
    readonly IFormatter<Points>               _jkt;

    public ComposeRequestJwk(ICurrentContext context) : this(context, JwsHeaderParser.Default, ComputeJkt.Default) {}

    public ComposeRequestJwk(ICurrentContext context, ISelect<HttpRequest, JwsResult?> parser, IFormatter<Points> jkt)
    {
        _context = context;
        _parser  = parser;
        _jkt     = jkt;
    }

    public JwkHeader? Get(string parameter)
    {
        var parsed = _parser.Get(_context.Get().Request);
        if (parsed is not null)
        {
            using var instance = parsed.Value;
            var       actual   = instance.Header.AsSpan();
            var       result   = JsonSerializer.Deserialize<JwsHeader>(actual, FrameworkSerializerOptions.Default)?.Jwk;
            if (result is not null && _jkt.Get(new(result.X, result.Y)) == parameter)
            {
                return result;
            }
        }

        return null;
    }
}