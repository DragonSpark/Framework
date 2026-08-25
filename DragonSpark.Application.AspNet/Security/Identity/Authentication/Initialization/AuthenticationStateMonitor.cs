using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class AuthenticationStateMonitor : IAuthenticationStateMonitor, IDisposable
{
	readonly AuthenticationStateChangedHandler _handler;
	readonly AuthenticationStateProvider       _provider;
	readonly HandleStateChange                 _change;

	public AuthenticationStateMonitor(AuthenticationStateProvider provider, HandleStateChange change)
		: this(provider, change, change.Execute) {}

	public AuthenticationStateMonitor(AuthenticationStateProvider provider, HandleStateChange change,
	                                  AuthenticationStateChangedHandler handler)
	{
		_provider = provider;
		_change   = change;
		_handler  = handler;
	}

	public ValueTask Get()
	{
		_provider.AuthenticationStateChanged += _handler;

		var start = _provider.GetAuthenticationStateAsync();
		return _change.Get(start);
	}

	public void Dispose()
	{
		_provider.AuthenticationStateChanged -= _handler;
	}
}