using System.Text.Json;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class AddOrUpdatePasskey<T> : ISelecting<JsonElement, IResult> where T : class
{
    readonly IAuthentications<T> _authentications;
    readonly ICurrentContext     _context;
    readonly PasskeySettings     _settings;

    protected AddOrUpdatePasskey(IAuthentications<T> authentications, ICurrentContext context, PasskeySettings settings)
    {
        _authentications = authentications;
        _context         = context;
        _settings        = settings;
    }

    public async ValueTask<IResult> Get(JsonElement parameter)
    {
        if (parameter.TryGetProperty("credentialJson", out var property) && property.ValueKind == JsonValueKind.String)
        {
            var context = _context.Get();
            context.Request.Host = _settings.Host is not null ? new(_settings.Host) : context.Request.Host;

            var credential = property.GetString();
            if (credential is not null)
            {
                using var session = _authentications.Get();
                var (subject, users) = session;
                var attest = await subject.PerformPasskeyAttestationAsync(credential).Off();
                if (attest.Succeeded)
                {
                    var user = await users.GetUserAsync(context.User).Off();
                    await users.AddOrUpdatePasskeyAsync(user.Verify(), attest.Passkey).Off();
                    return Results.Ok();
                }

                return Results.BadRequest("Passkey attestation failed");
            }

            return Results.BadRequest("No credential found");
        }

        return Results.BadRequest("Missing or invalid credentialJson");
    }
}