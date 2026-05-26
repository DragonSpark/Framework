using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;

public sealed class AmrClaim : IBearerClaim
{
    public static AmrClaim Default { get; } = new();

    AmrClaim() {}

    public Claim Get(ClaimsIdentity parameter)
        => new(JwtRegisteredClaimNames.Amr, parameter.AuthenticationType.Verify());
}