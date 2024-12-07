using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity;

public sealed class CurrentProviderIdentity : SelectedResult<ClaimsPrincipal, ProviderIdentity>
{
	public CurrentProviderIdentity(ICurrentPrincipal previous) : base(previous, Identities.Default) {}
}