using DragonSpark.Application.AspNet.Security.Identity.Claims.Access;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class ReadIdentityProvider : ReadClaim
{
	public static ReadIdentityProvider Default { get; } = new();

	ReadIdentityProvider() : base(ClaimTypes.AuthenticationMethod) {}
}