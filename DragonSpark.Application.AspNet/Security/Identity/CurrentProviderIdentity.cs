using DragonSpark.Application.Security.Identity;
using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class CurrentProviderIdentity : SelectedResult<ClaimsPrincipal, ProviderIdentity>
{
	public CurrentProviderIdentity(ICurrentPrincipal previous) : base(previous, Identities.Default) {}
}