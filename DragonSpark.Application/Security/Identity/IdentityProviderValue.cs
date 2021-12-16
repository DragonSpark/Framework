using DragonSpark.Application.Security.Identity.Claims.Access;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

public sealed class IdentityProviderValue : RequiredClaim
{
	public static IdentityProviderValue Default { get; } = new();

	IdentityProviderValue() : base(ClaimTypes.AuthenticationMethod) {}
}