using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public class ExternalChallenge<T> : IExternalChallenge where T : class
{
    readonly IAuthentications<T> _authentications;
    readonly ICurrentContext     _context;
    readonly string              _action;

    protected ExternalChallenge(IAuthentications<T> authentications, ICurrentContext context, string action)
    {
        _authentications = authentications;
        _context         = context;
        _action          = action;
    }

    public IResult Get(PerformExternalLoginInput parameter)
    {
        var (provider, returnAddress) = parameter;
        IEnumerable<KeyValuePair<string, StringValues>> query =
        [
            new("ReturnUrl", returnAddress),
            new("Action", _action)
        ];
        var       context    = _context.Get();
        var       create     = QueryString.Create(query);
        var       address    = UriHelper.BuildRelative(context.Request.PathBase, "/Account/ExternalLogin", create);
        using var session    = _authentications.Get();
        var       properties = session.Subject.ConfigureExternalAuthenticationProperties(provider, address);
        return TypedResults.Challenge(properties, [provider]);
    }
}