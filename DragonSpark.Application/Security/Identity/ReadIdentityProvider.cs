using DragonSpark.Application.Security.Identity.Claims.Access;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

sealed class ReadIdentityProvider : ReadClaim
{
	public static ReadIdentityProvider Default { get; } = new();

	ReadIdentityProvider() : base(ClaimTypes.AuthenticationMethod) {}
}