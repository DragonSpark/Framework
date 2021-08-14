using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Claims.Access;

namespace DragonSpark.Presentation.Security.Identity
{
	public sealed class CurrentAuthenticationMethod : CurrentClaimValue
	{
		public CurrentAuthenticationMethod(ICurrentPrincipal source) : base(source, AuthenticationMethod.Default) {}
	}
}