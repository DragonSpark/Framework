using DragonSpark.Application.Security.Identity.Claims;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class ReadIdentityProvider : ReadClaim
{
	public static ReadIdentityProvider Default { get; } = new();

	ReadIdentityProvider() : base(ClaimTypes.AuthenticationMethod) {}
}