using DragonSpark.Application.Security.Identity.Claims;
using Duende.IdentityModel;

namespace DragonSpark.Application.Mobile.Security.Identity.Profile;

sealed class LastName : RequiredClaim
{
	public static LastName Default { get; } = new();

	LastName() : base(JwtClaimTypes.FamilyName) {}
}