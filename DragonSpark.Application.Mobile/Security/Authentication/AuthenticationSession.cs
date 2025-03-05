using System.Collections.Specialized;
using System.Text.Json.Serialization;
using DragonSpark.Application.Runtime.Process;

namespace DragonSpark.Application.Mobile.Security.Authentication;

[method: JsonConstructor]
public sealed record AuthenticationSession(uint Process, string Identifier, string? State)
{
    public AuthenticationSession(string Identifier, NameValueCollection Query)
        : this(Identifier, Query["state"] is {} previous && !string.IsNullOrEmpty(previous) ? previous : null) {}

    public AuthenticationSession(string Identifier, string? State)
        : this(GetProcessIdentity.Default, Identifier, State) {}
}