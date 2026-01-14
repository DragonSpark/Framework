using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Http;

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
                   ? TypedResults.Content(await _options.Off(new(context, signin, user)), "application/json")
                   : Results.Unauthorized();
    }
}

// TODO

public class HasPasskeys<T> : IDepending<string> where T : class
{
    readonly IUsers<T> _users;

    protected HasPasskeys(IUsers<T> users) => _users = users;

    public async ValueTask<bool> Get(Stop<string> parameter)
    {
        using var session = _users.Get();
        var       user    = await session.Subject.FindByEmailAsync(parameter).Off();
        var       result  = user is not null && await session.Subject.GetPasskeysAsync(user).Off() is { Count: > 0 };
        return result;
    }
}