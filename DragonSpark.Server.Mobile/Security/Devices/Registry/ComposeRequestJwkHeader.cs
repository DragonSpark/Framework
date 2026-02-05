using DragonSpark.Application.AspNet.Security;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class ComposeRequestJwkHeader : IResult<JwkHeader?>
{
    readonly ICurrentContext                  _context;
    readonly ISelect<HttpRequest, JwkHeader?> _jwk;

    public ComposeRequestJwkHeader(ICurrentContext context) : this(context, ComposeJwk.Default) {}

    public ComposeRequestJwkHeader(ICurrentContext context, ISelect<HttpRequest, JwkHeader?> jwk)
    {
        _context = context;
        _jwk     = jwk;
    }

    public JwkHeader? Get() => _jwk.Get(_context.Get().Request);
}