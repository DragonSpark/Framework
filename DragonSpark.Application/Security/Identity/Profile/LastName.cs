using System.IdentityModel.Tokens.Jwt;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class LastName : RequiredClaim
{
	public static LastName Default { get; } = new();

	LastName() : base(JwtRegisteredClaimNames.FamilyName) {}
}