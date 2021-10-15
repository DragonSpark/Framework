using DragonSpark.Model.Commands;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model;

sealed class AuthenticationStateAwareNavigateToSignOut : AppendedCommand<ClaimsPrincipal>, INavigateToSignOut
{
	public AuthenticationStateAwareNavigateToSignOut(INavigateToSignOut previous, IClearAuthenticationState clear)
		: base(previous, clear) {}
}