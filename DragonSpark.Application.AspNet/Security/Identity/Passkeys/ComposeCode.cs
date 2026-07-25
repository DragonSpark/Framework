using System.Text.Json;
using DragonSpark.Application.AspNet.Security.Identity.Bearer;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class ComposeCode : IAlteration<string>
{
    readonly ISecureToken _token;
    readonly string       _type;
    readonly TimeSpan     _expires;

    public ComposeCode(ISecureToken token) : this(token, ResponseType.Default, DefaultExpiration.Default) {}

    public ComposeCode(ISecureToken token, string type, TimeSpan expires)
    {
        _token   = token;
        _type    = type;
        _expires = expires;
    }

    public string Get(string parameter)
    {
        var code   = _token.Get(new([new(_type, parameter)], _expires));
        var result = JsonSerializer.Serialize(new { code });
        return result;
    }
}