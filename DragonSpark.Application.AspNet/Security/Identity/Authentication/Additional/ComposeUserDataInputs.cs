using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public sealed class ComposeUserDataInputs<T> : IComposeUserDataInputs<T> where T : class
{
    readonly IUsers<T>                         _users;
    readonly ICurrentContext                   _context;
    readonly ILogger<ComposeUserDataInputs<T>> _logger;

    public ComposeUserDataInputs(IUsers<T> users, ICurrentContext context, ILogger<ComposeUserDataInputs<T>> logger)
    {
        _users   = users;
        _context = context;
        _logger  = logger;
    }

    public async ValueTask<ComposeUserDataInput<T>> Get(CancellationToken parameter)
    {
        var context = _context.Get();
        var users   = _users.Get();
        var user    = await users.Subject.GetUserAsync(context.User).Off();
        if (user is not null)
        {
            var id = await users.Subject.GetUserIdAsync(user).Off();
            _logger.LogInformation("User with ID '{UserId}' asked for their personal data", id);
            context.Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");
        }

        return new(users, user, context);
    }
}