using DragonSpark.Application.Security.Identity.Claims;
using Duende.IdentityModel;

namespace DragonSpark.Application.Mobile.Security.Identity.Profile;

sealed class Identifier : RequiredClaim
{
	public static Identifier Default { get; } = new();

	Identifier() : base(JwtClaimTypes.Subject) {}
}