using System.Text.Json.Serialization;

namespace DragonSpark.Contracts.Security;

[method: JsonConstructor]
public sealed record AccessTokenView(string Identifier, DateTimeOffset Expiration, AccessTokenResponse Response)
{
    public AccessTokenView(string identifier, AccessTokenResponse response)
        : this(identifier, DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn), response) {}
}