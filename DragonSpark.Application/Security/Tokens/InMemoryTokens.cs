using System;
using System.Collections.Generic;
using DragonSpark.Model;

namespace DragonSpark.Application.Security.Tokens;

public sealed class InMemoryTokens : ITokens
{
    readonly IDictionary<string, string> _map;

    public InMemoryTokens() : this(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)) {}

    public InMemoryTokens(IDictionary<string, string> map) => _map = map;

    public string? Get(Uri origin) => _map.TryGetValue(origin.GetLeftPart(UriPartial.Authority), out var n) ? n : null;

    public void Execute(Pair<Uri, string> parameter)
    {
        var (origin, token)                            = parameter;
        _map[origin.GetLeftPart(UriPartial.Authority)] = token;
    }
}