using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

public sealed class IsCurrentProvider : Equaling<ProviderIdentity>
{
	public IsCurrentProvider(ICurrentPrincipal current) : base(current.Then().Select(x => x.Identity())) {}
}