using System;
using System.Text.Json.Serialization;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Communication.Http.Security;

[method: JsonConstructor]
public sealed record AccessTokenView(string identifier, DateTimeOffset Expiration, AccessTokenResponse Response)
{
    public AccessTokenView(string identifier, AccessTokenResponse response)
        : this(identifier, Time.Default.Get().AddSeconds(response.ExpiresIn), response) {}
}