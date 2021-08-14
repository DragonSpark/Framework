using DragonSpark.Application.Security.Identity.Claims.Access;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class Identities : Select<ClaimsPrincipal, ProviderIdentity>
	{
		public static Identities Default { get; } = new Identities();

		Identities() : this(ExternalIdentityValue.Default, IdentityParser.Default) {}

		public Identities(IRequiredClaim identity, ISelect<string, ProviderIdentity> parser)
			: base(identity.Then().Select(parser)) {}
	}
}