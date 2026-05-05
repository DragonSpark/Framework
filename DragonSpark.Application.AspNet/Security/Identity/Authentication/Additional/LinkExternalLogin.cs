using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public class LinkExternalLogin<T> : ILinkExternalLogin where T : class
{
    readonly IAuthentications<T> _authentications;
    readonly ICurrentContext     _context;
    readonly QueryString         _action;

    protected LinkExternalLogin(IAuthentications<T> authentications, ICurrentContext context, string path)
        : this(authentications, context, QueryString.Create("Action", path)) {}

    protected LinkExternalLogin(IAuthentications<T> authentications, ICurrentContext context,
                                QueryString action)
    {
        _authentications = authentications;
        _context         = context;
        _action          = action;
    }

    public async ValueTask<IResult> Get(string parameter)
    {
        var context = _context.Get();
        await context.SignOutAsync(IdentityConstants.ExternalScheme).Off();

        var       path       = context.Request.PathBase;
        var       address    = UriHelper.BuildRelative(path, "/Account/Manage/ExternalLogins", _action);
        using var session    = _authentications.Get();
        var       id         = session.Subject.UserManager.GetUserId(context.User);
        var       properties = session.Subject.ConfigureExternalAuthenticationProperties(parameter, address, id);
        return TypedResults.Challenge(properties, [parameter]);
    }
}