using System.IdentityModel.Tokens.Jwt;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class FullName : RequiredClaim
{
	public static FullName Default { get; } = new();

	FullName() : base(JwtRegisteredClaimNames.Name) {}
}