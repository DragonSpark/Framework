using DragonSpark.Application.Security.Identity.Claims.Access;

namespace DragonSpark.Application.Security.Identity.Claims;

public sealed class CurrentAuthenticationMethod : CurrentClaimValue
{
	public CurrentAuthenticationMethod(ICurrentPrincipal source) : base(source, AuthenticationMethod.Default) {}
}