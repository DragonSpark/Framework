using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class AuthenticationStateSource<T> : CascadingValueSource<AuthenticationState<T>> where T : IdentityUser
{
	public AuthenticationStateSource() : base(AuthenticationState<T>.Default, false) {}
}
sealed class AuthenticationStateSource : CascadingValueSource<Task<AuthenticationState>>
{
	public AuthenticationStateSource(IAdapters adapters, AuthenticationStateProvider provider)
		: base(new AdaptedStateProvider(provider, adapters).Get, false) {}
}