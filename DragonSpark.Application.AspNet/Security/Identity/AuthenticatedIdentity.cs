using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

sealed class AuthenticatedIdentity : Select<ClaimsPrincipal, ProviderIdentity>
{
	public static AuthenticatedIdentity Default { get; } = new();

	AuthenticatedIdentity()
		: base(AuthenticationIdentifier.Default.Then().Verified().Select(ProviderIdentityParser.Default)) {}
}