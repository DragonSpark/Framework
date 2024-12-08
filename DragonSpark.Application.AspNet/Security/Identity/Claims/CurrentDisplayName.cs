using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

public sealed class CurrentDisplayName : CurrentClaimValue
{
	public CurrentDisplayName(ICurrentPrincipal source)
		: base(source, Access.DisplayName.Default, x => x.ValueOrDefault(Anonymous.Default)) {}
}