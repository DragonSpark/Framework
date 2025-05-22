using System;
using System.Text.Json.Serialization;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Communication.Http.Security;

[method: JsonConstructor]
public sealed record AccessTokenView(string Identifier, DateTimeOffset Expiration, AccessTokenResponse Response)
{
    public AccessTokenView(string Identifier, AccessTokenResponse Response)
        : this(Identifier, Time.Default.Get().AddSeconds(Response.ExpiresIn), Response) {}
}