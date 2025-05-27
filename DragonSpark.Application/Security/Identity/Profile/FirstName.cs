using System.IdentityModel.Tokens.Jwt;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class FirstName : RequiredClaim
{
	public static FirstName Default { get; } = new();

	FirstName() : base(JwtRegisteredClaimNames.GivenName) {}
}