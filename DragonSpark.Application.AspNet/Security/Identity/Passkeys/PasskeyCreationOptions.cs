using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class PasskeyCreationOptions<T> : IResulting<IResult> where T : IdentityUser
{
    readonly IAuthentications<T>               _authentications;
    readonly ICurrentContext                   _context;
    readonly IComposePasskeyCreationOptions<T> _options;

    protected PasskeyCreationOptions(IAuthentications<T> authentications, ICurrentContext context)
        : this(authentications, context, ComposePasskeyCreationOptions<T>.Default) {}

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