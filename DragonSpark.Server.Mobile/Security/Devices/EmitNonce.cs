using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class EmitNonce<T> : Model.Operations.Stop.IStopAware<HttpContext> where T : Nonce
{
    readonly CreateNonce<T> _previous;
    readonly string      _header;

    public EmitNonce(CreateNonce<T> previous) : this(previous, DpopNonceHeaderName.Default) {}

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