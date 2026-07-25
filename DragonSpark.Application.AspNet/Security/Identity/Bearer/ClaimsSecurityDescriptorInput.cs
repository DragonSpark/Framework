using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public readonly record struct ClaimsSecurityDescriptorInput(
    IDictionary<string, object> Claims,
    TimeSpan? Expiration = null)
{
    public ClaimsSecurityDescriptorInput(ClaimsIdentity identity, TimeSpan? expiration = null)
        : this(identity.Claims, expiration) {}

    public ClaimsSecurityDescriptorInput(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        : this(claims.ToDictionary(x => x.Type, object (x) => x.Value), expiration) {}
}