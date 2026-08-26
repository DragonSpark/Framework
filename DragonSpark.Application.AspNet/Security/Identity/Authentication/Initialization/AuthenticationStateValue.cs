using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class AuthenticationStateValue<T> : IResult<AuthenticationState<T>?> where T : IdentityUser
{
	readonly AdaptedStateProvider _provider;

	public AuthenticationStateValue(AdaptedStateProvider provider) => _provider = provider;

	public AuthenticationState<T>? Get()
		=> _provider.Get() is { IsCompletedSuccessfully: true, Result: AuthenticationState<T> result } ? result : null;
}