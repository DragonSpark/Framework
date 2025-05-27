using System.IdentityModel.Tokens.Jwt;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class Subject : RequiredClaim
{
    public static Subject Default { get; } = new();

    Subject() : base(JwtRegisteredClaimNames.Sub) {}
}