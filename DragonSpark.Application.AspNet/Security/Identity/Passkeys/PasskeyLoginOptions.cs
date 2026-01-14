using System;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class PasskeyLoginOptions<T> : ISelecting<string, IResult> where T : class
{
    readonly IAuthentications<T> _sessions;
    readonly PasskeySettings     _settings;
    readonly ICurrentContext     _context;

    public PasskeyLoginOptions(IAuthentications<T> sessions, PasskeySettings settings, ICurrentContext context)
    {
        _sessions = sessions;
        _settings = settings;
        _context  = context;
    }

    public async ValueTask<IResult> Get(string parameter)
    {
        using var session = _sessions.Get();
        var (subject, users) = session;
        var user = await users.FindByEmailAsync(parameter).Off();
        if (user is not null)
        {
            var options = await subject.MakePasskeyRequestOptionsAsync(null).Off();
            var replace = _settings.Host ?? _context.Get().Request.Host.Host;
            var result  = options.Replace(@"""id"":""localhost""", $@"""id"":""{replace}""");
            return Results.Content(result);
        }

        return Results.Ok(new { allowCredentials = Array.Empty<object>() });
    }
}