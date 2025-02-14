using DragonSpark.Application.Security.Identity.Claims;
using Duende.IdentityModel;

namespace DragonSpark.Application.Mobile.Security.Identity.Profile;

sealed class FullName : RequiredClaim
{
	public static FullName Default { get; } = new();

	FullName() : base(JwtClaimTypes.Name) {}
}