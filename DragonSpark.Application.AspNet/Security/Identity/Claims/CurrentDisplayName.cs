using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Claims.Access;

namespace DragonSpark.Application.Security.Identity.Claims;

public sealed class CurrentDisplayName : CurrentClaimValue
{
	public CurrentDisplayName(ICurrentPrincipal source)
		: base(source, Access.DisplayName.Default, x => x.ValueOrDefault(Anonymous.Default)) {}
}