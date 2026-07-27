using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices.Nonce;

sealed class EmitNonce<T> : Model.Operations.Stop.IStopAware<HttpContext>
    where T : Application.AspNet.Security.Tokens.Nonce
{
    readonly CreateNonce<T> _previous;
    readonly string         _header;

    public EmitNonce(CreateNonce<T> previous) : this(previous, TokenName.Default) {}

    public EmitNonce(CreateNonce<T> previous, string header)
    {
        _previous = previous;
        _header   = header;
    }

    public async ValueTask Get(Stop<HttpContext> parameter)
    {
        var (subject, _)                  = parameter;
        subject.Response.Headers[_header] = await _previous.Off(parameter);
    }
}