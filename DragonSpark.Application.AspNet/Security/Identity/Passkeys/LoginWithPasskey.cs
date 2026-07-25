using System.Text.Json;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class LoginWithPasskey<T> : ISelecting<JsonElement, IResult> where T : class
{
    readonly IAuthentications<T> _authentications;
    readonly string              _scheme;

    protected LoginWithPasskey(IAuthentications<T> authentications)
        : this(authentications, IdentityConstants.BearerScheme) {}

    protected LoginWithPasskey(IAuthentications<T> authentications, string scheme)
    {
        _authentications = authentications;
        _scheme          = scheme;
    }

    public async ValueTask<IResult> Get(JsonElement parameter)
    {
        if (parameter.TryGetProperty("credentialJson", out var element) && element.ValueKind == JsonValueKind.String)
        {
            var content = element.GetString();
            if (content is not null)
            {
                using var session = _authentications.Get();
                session.Subject.AuthenticationScheme = _scheme;
                var result = await session.Subject.PasskeySignInAsync(content).Off();
                return result.Succeeded ? Results.Ok() : Results.Unauthorized();
            }
        }

        return Results.BadRequest("Missing or invalid credentialJson");
    }
}