using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Navigation.Security;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class PasskeyCreationOptions<T> : IResulting<IResult> where T : IdentityUser
{
    readonly IAuthentications<T>               _authentications;
    readonly ICurrentContext                   _context;
    readonly IComposePasskeyCreationOptions<T> _options;

    protected PasskeyCreationOptions(IAuthentications<T> authentications, ICurrentContext context,
                                     IComposePasskeyCreationOptions<T> options)
    {
        _authentications = authentications;
        _context         = context;
        _options         = options;
    }

    public async ValueTask<IResult> Get()
    {
        using var session = _authentications.Get();
        var       signin  = session.Subject;
        var       users   = session.Users;
        var       context = _context.Get();
        var       user    = await users.GetUserAsync(context.User).Off();
        return user is not null
                   ? Results.Content(await _options.Off(new(signin, user)), "application/json")
                   : Results.NotFound($"Unable to load user with ID '{users.GetUserId(context.User)}'.");
    }
}

// TODO

public readonly record struct PerformExternalLoginInput(string Provider, string? ReturnAddress);

public interface IPerformExternalLogin : ISelect<PerformExternalLoginInput, IResult>;

public class PerformExternalLogin<T> : IPerformExternalLogin where T : class
{
    readonly IAuthentications<T> _authentications;
    readonly ICurrentContext     _context;

    protected PerformExternalLogin(IAuthentications<T> authentications, ICurrentContext context)
    {
        _authentications = authentications;
        _context         = context;
    }

    public IResult Get(PerformExternalLoginInput parameter)
    {
        var (provider, returnAddress) = parameter;
        IEnumerable<KeyValuePair<string, StringValues>> query =
        [
            new("ReturnUrl", returnAddress),
            new("Action", ExternalLoginPath.Default.Get())
        ];
        var       context    = _context.Get();
        var       create     = QueryString.Create(query);
        var       address    = UriHelper.BuildRelative(context.Request.PathBase, "/Account/ExternalLogin", create);
        using var session    = _authentications.Get();
        var       properties = session.Subject.ConfigureExternalAuthenticationProperties(provider, address);
        return TypedResults.Challenge(properties, [provider]);
    }
}

public interface ILinkExternalLogin : ISelecting<string, IResult>;
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