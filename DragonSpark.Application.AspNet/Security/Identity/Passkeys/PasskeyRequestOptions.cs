using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class PasskeyRequestOptions<T> : ISelecting<string?, IResult> where T : class
{
    readonly IAuthentications<T> _authentications;

    protected PasskeyRequestOptions(IAuthentications<T> authentications) => _authentications = authentications;

    public async ValueTask<IResult> Get(string? parameter)
    {
        using var session = _authentications.Get();
        var       user    = !parameter.IsNullOrEmpty() ? await session.Users.FindByNameAsync(parameter).Off() : null;
        var       content = await session.Subject.MakePasskeyRequestOptionsAsync(user).Off();
        return TypedResults.Content(content, "application/json");
    }
}