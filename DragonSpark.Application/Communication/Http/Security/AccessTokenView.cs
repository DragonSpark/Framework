using System;
using System.Text.Json.Serialization;
using DragonSpark.Contracts.Security;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Communication.Http.Security;

[method: JsonConstructor]
public sealed record AccessTokenView(string Identifier, DateTimeOffset Expiration, AccessTokenResponse Response) // TODO: Move to Contracts
{
    public AccessTokenView(string identifier, AccessTokenResponse response)
        : this(identifier, Time.Default.Get().AddSeconds(response.ExpiresIn), response) {}
}