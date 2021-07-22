using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class CurrentProviderIdentity : DelegatedSelection<ClaimsPrincipal, ProviderIdentity>
	{
		public CurrentProviderIdentity(ICurrentPrincipal parameter) : base(Identities.Default, parameter) {}
	}
}