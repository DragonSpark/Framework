using System;
using System.Text.Json;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Security.Authentication;

sealed class ParseSession : ISelect<Uri, AuthenticationSession>
{
    public static ParseSession Default { get; } = new();

    ParseSession() : this(ParseQuery.Default, ParseFragment.Default) {}

    readonly ISelect<Uri, string?> _query;
    readonly ISelect<Uri, string?> _fragment;

    public ParseSession(ISelect<Uri, string?> query, ISelect<Uri, string?> fragment)
    {
        _query    = query;
        _fragment = fragment;
    }

    public AuthenticationSession Get(Uri parameter)
    {
        var state = _query.Get(parameter) ?? _fragment.Get(parameter)
                    ?? throw new InvalidOperationException("Could not locate state");
        var session = JsonSerializer.Deserialize<AuthenticationSession>(state);
        return session.Verify();
    }
}