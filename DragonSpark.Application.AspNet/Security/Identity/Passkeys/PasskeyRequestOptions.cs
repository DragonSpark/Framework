using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class PasskeyRequestOptions<T> : IPasskeyRequestOptions where T : class
{
    readonly IAuthentications<T> _authentications;

    protected PasskeyRequestOptions(IAuthentications<T> authentications) => _authentications = authentications;

    public async ValueTask<IResult> Get(string? parameter)
    {
        using var session = _authentications.Get();
        var       user    = !parameter.IsNullOrEmpty() ? await session.Users.FindByNameAsync(parameter).Off() : null;
        var       content = await session.Subject.MakePasskeyRequestOptionsAsync(user).Off();
        return Results.Content(content, "application/json");
    }
}