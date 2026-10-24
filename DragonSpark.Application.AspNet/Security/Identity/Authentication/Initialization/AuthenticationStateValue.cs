using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class AuthenticationStateValue<T> : IResult<AuthenticationState<T>> where T : IdentityUser
{
	readonly AdaptedStateProvider   _provider;
	readonly AuthenticationState<T> _default;

	public AuthenticationStateValue(AdaptedStateProvider provider)
		: this(provider, AuthenticationState<T>.Default) {}

	public AuthenticationStateValue(AdaptedStateProvider provider, AuthenticationState<T> @default)
	{
		_provider = provider;
		_default  = @default;
	}

	public AuthenticationState<T> Get()
		=> _provider.Get() is { IsCompletedSuccessfully: true, Result: AuthenticationState<T> result }
			   ? result
			   : _default;
}