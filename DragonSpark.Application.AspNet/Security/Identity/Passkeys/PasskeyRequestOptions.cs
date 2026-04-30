using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

// TODO
public interface IPasskeyRequestOptions : ISelecting<string?, IResult>;

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

public class DownloadPersonalData<T> : IResulting<IResult> where T : class
{
    readonly IComposeUserDataInputs<T> _inputs;
    readonly IFormatUserData<T>        _format;

    public DownloadPersonalData(IComposeUserDataInputs<T> inputs) : this(inputs, FormatUserData<T>.Default) {}

    public DownloadPersonalData(IComposeUserDataInputs<T> inputs, IFormatUserData<T> format)
    {
        _inputs = inputs;
        _format = format;
    }

    public async ValueTask<IResult> Get()
    {
        using var inputs = await _inputs.Off();
        var (users, user, context) = inputs;
        return user is not null
                   ? TypedResults.File(await _format.Off(inputs), contentType: "application/json",
                                       fileDownloadName: "PersonalData.json")
                   : Results.NotFound($"Unable to load user with ID '{users.Subject.GetUserId(context.User)}'.");
    }
}

public interface IFormatUserData<T> : ISelecting<ComposeUserDataInput<T>, Array<byte>> where T : class;

sealed class FormatUserData<T> : IFormatUserData<T> where T : class
{
    public static FormatUserData<T> Default { get; } = new();

    FormatUserData() : this(ComposeUserData<T>.Default) {}

    readonly ISelecting<ComposeUserDataInput<T>, IReadOnlyDictionary<string, string>> _compose;

    public FormatUserData(ISelecting<ComposeUserDataInput<T>, IReadOnlyDictionary<string, string>> compose)
        => _compose = compose;

    public async ValueTask<Array<byte>> Get(ComposeUserDataInput<T> parameter)
    {
        var data   = await _compose.Off(parameter);
        var result = JsonSerializer.SerializeToUtf8Bytes(data);
        return result;
    }
}

public interface IComposeUserDataInputs<T> : IResulting<ComposeUserDataInput<T>> where T : class;

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

    public async ValueTask<ComposeUserDataInput<T>> Get()
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

public readonly record struct ComposeUserDataInput<T>(UsersSession<T> Users, T? User, HttpContext Context)
    : IDisposable where T : class
{
    public void Dispose()
    {
        Users.Dispose();
    }
}

sealed class ComposeUserData<T> : ISelecting<ComposeUserDataInput<T>, IReadOnlyDictionary<string, string>>
    where T : class
{
    public static ComposeUserData<T> Default { get; } = new();

    ComposeUserData()
        : this(A.Type<T>()
                .GetProperties()
                .Where(x => Attribute.IsDefined(x, typeof(PersonalDataAttribute)))
                .ToArray(), "null") {}

    readonly Array<PropertyInfo> _properties;
    readonly string              _unassigned;

    public ComposeUserData(Array<PropertyInfo> properties, string unassigned)
    {
        _properties = properties;
        _unassigned = unassigned;
    }

    public async ValueTask<IReadOnlyDictionary<string, string>> Get(ComposeUserDataInput<T> parameter)
    {
        var (users, user, _) = parameter;

        var result = new Dictionary<string, string>();
        foreach (var p in _properties)
        {
            result.Add(p.Name, p.GetValue(parameter)?.ToString() ?? _unassigned);
        }

        var verify = user.Verify();
        var logins = await users.Subject.GetLoginsAsync(verify).Off();
        foreach (var l in logins)
        {
            result.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
        }

        var key = await users.Subject.GetAuthenticatorKeyAsync(verify).Off();

        result.Add("Authenticator Key", key!);

        return result;
    }
}