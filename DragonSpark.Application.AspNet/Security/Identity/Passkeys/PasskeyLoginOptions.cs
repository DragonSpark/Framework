using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class PasskeyLoginOptions<T> : ISelecting<string, IResult> where T : class
{
    readonly IAuthentications<T> _sessions;

    protected PasskeyLoginOptions(IAuthentications<T> sessions) => _sessions = sessions;

    public async ValueTask<IResult> Get(string parameter)
    {
        using var session = _sessions.Get();
        var (subject, users) = session;
        var user = await users.FindByEmailAsync(parameter).Off();
        if (user is not null)
        {
            var result = await subject.MakePasskeyRequestOptionsAsync(user).Off();
            return Results.Content(result, "application/json");
        }

        return Results.Conflict();
    }
}