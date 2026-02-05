using System.Text.Json;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Model.Selection;
using DragonSpark.Server.Mobile.Security.Devices.Authentication;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class ComposeJwk : ISelect<HttpRequest, JwkHeader?>
{
    public static ComposeJwk Default { get; } = new();

    ComposeJwk() : this(JwsHeaderParser.Default, FrameworkSerializerOptions.Default) {}

    readonly ISelect<HttpRequest, ParsedJws?> _parser;
    readonly JsonSerializerOptions            _options;

    public ComposeJwk(ISelect<HttpRequest, ParsedJws?> parser, JsonSerializerOptions options)
    {
        _parser  = parser;
        _options = options;
    }

    public JwkHeader? Get(HttpRequest parameter)
    {
        var parsed = _parser.Get(parameter);
        if (parsed is not null)
        {
            using var instance = parsed.Value;
            var       actual   = instance.Header.AsSpan();
            var       result   = JsonSerializer.Deserialize<JwsHeader>(actual, _options)?.Jwk;
            return result;
        }

        return null;
    }
}