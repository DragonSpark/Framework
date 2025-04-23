using System;
using System.Text.Json;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Mobile.Uno.Security.Authentication;

sealed class ComposeSession : IAlteration<Session>
{
    public static ComposeSession Default { get; } = new();

    ComposeSession() {}

    public Session Get(Session parameter)
    {
        var (identifier, address) = parameter;
        var builder = new UriBuilder(address);
        var query   = System.Web.HttpUtility.ParseQueryString(address.Query);
        query["state"] = JsonSerializer.Serialize(new AuthenticationSession(identifier, query));
        builder.Query  = query.ToString();
        return parameter with { Address = builder.Uri };
    }
}