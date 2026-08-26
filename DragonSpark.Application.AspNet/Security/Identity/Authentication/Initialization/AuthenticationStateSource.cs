using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class AuthenticationStateSource<T> : CascadingValueSource<AuthenticationState<T>?> where T : IdentityUser
{
	public AuthenticationStateSource(AuthenticationStateValue<T> value) : base(value.Get, false) {}
}

sealed class AuthenticationStateSource : CascadingValueSource<Task<AuthenticationState>>
{
	public AuthenticationStateSource(AdaptedStateProvider provider) : base(provider.Get, false) {}
}