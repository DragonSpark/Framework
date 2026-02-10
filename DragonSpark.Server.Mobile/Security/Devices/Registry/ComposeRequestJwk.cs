using DragonSpark.Application.Security.Tokens;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class ComposeRequestJwk : IParser<JwkHeader?>
{
    readonly ComposeRequestJwkHeader _header;
    readonly IFormatter<Points>      _jkt;

    public ComposeRequestJwk(ComposeRequestJwkHeader header) : this(header, ComputeJkt.Default) {}

    public ComposeRequestJwk(ComposeRequestJwkHeader header, IFormatter<Points> jkt)
    {
        _header = header;
        _jkt    = jkt;
    }

    public JwkHeader? Get(string parameter)
    {
        var header = _header.Get();
        var result = header is not null && _jkt.Get(new(header.X, header.Y)) == parameter ? header : null;
        return result;
    }
}