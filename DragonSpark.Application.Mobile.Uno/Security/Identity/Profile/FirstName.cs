using DragonSpark.Application.Security.Identity.Claims;
using Duende.IdentityModel;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity.Profile;

sealed class FirstName : RequiredClaim
{
	public static FirstName Default { get; } = new();

	FirstName() : base(JwtClaimTypes.GivenName) {}
}