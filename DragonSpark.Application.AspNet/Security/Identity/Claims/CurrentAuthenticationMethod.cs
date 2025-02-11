using DragonSpark.Application.AspNet.Security.Identity.Claims.Access;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

public sealed class CurrentAuthenticationMethod : CurrentClaimValue
{
	public CurrentAuthenticationMethod(ICurrentPrincipal source) : base(source, AuthenticationMethod.Default) {}
}