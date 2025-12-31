using System;
using System.Text.Json;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

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
        var result = user is not null
                         ? TypedResults.Content(await _options.Off(new(context, signin, user)), "application/json")
                         : Results.Unauthorized();
        return result;
    }
}

public readonly record struct ComposePasskeyCreationOptionsInput<T>(
    HttpContext Context,
    SignInManager<T> SignIn,
    UserManager<T> User,
    T Subject) where T : class
{
    public ComposePasskeyCreationOptionsInput(HttpContext Context, SignInManager<T> SignIn, T Subject)
        : this(Context, SignIn, SignIn.UserManager, Subject) {}
}

public interface IComposePasskeyCreationOptions<T> : ISelecting<ComposePasskeyCreationOptionsInput<T>, string>
    where T : class;

sealed class ComposePasskeyCreationOptions<T> : IComposePasskeyCreationOptions<T> where T : class
{
    readonly PasskeySettings _settings;

    public ComposePasskeyCreationOptions(PasskeySettings settings) => _settings = settings;

    public async ValueTask<string> Get(ComposePasskeyCreationOptionsInput<T> parameter)
    {
        var (context, signIn, users, subject) = parameter;
        var userId   = await users.GetUserIdAsync(subject).Off();
        var userName = await users.GetUserNameAsync(subject).Off() ?? "User";
        var entity   = new PasskeyUserEntity { Id = userId, Name = userName, DisplayName = userName };
        var options  = await signIn.MakePasskeyCreationOptionsAsync(entity).Off();
        var replace  = _settings.Host ?? context.Request.Host.Host;
        var result = options.Replace(@"""id"":""localhost""", $@"""id"":""{replace}""")
                            .Replace(@"""name"":""localhost""", $@"""name"":""{_settings.Name}""");
        return result;
    }
}

// TODO

public class PasskeyLoginOptions<T> : ISelecting<LoginRequest, IResult> where T : class
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

    public async ValueTask<IResult> Get(LoginRequest parameter)
    {
        using var session = _sessions.Get();
        var (subject, users) = session;
        var user = await users.FindByEmailAsync(parameter.Email).Off();
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

public sealed record LoginRequest(string Email);

public class LoginWithPasskey<T> : ISelecting<JsonElement, IResult> where T : class
{
    readonly IAuthentications<T> _authentications;

    protected LoginWithPasskey(IAuthentications<T> authentications) => _authentications = authentications;

    public async ValueTask<IResult> Get(JsonElement parameter)
    {
        if (parameter.TryGetProperty("credentialJson", out var element) && element.ValueKind == JsonValueKind.String)
        {
            var content = element.GetString();
            if (content is not null)
            {
                using var session = _authentications.Get();
                var       result  = await session.Subject.PasskeySignInAsync(content).Off();
                return result.Succeeded ? Results.Ok() : Results.Unauthorized();
            }
        }

        return Results.BadRequest("Missing or invalid credentialJson");
    }
}

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
                var       attest  = await session.Subject.PerformPasskeyAttestationAsync(credential).Off();
                return attest.Succeeded ? Results.Ok() : Results.BadRequest("Passkey attestation failed");
            }

            return Results.BadRequest("No credential found");
        }

        return Results.BadRequest("Missing or invalid credentialJson");
    }
}