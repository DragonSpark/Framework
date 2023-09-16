using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class RefreshAuthenticationDisplayState : ICommand<ClaimsPrincipal>
{
	readonly IClearAuthenticationState                   _clear;
	readonly AuthenticationStateProvider                 _service;
	readonly IHostEnvironmentAuthenticationStateProvider _environment;

	public RefreshAuthenticationDisplayState(IClearAuthenticationState clear, AuthenticationStateProvider service)
		: this(clear, service, service.To<IHostEnvironmentAuthenticationStateProvider>()) {}

	public RefreshAuthenticationDisplayState(IClearAuthenticationState clear, AuthenticationStateProvider service,
	                                         IHostEnvironmentAuthenticationStateProvider environment)
	{
		_clear       = clear;
		_service     = service;
		_environment = environment;
	}

	public void Execute(ClaimsPrincipal parameter)
	{
		_clear.Execute(parameter);
		_environment.SetAuthenticationState(_service.GetAuthenticationStateAsync());
	}
}