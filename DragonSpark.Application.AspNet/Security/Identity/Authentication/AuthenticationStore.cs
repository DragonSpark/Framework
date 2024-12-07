using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Results;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class AuthenticationStore : IResulting<AuthenticationState>, ICommand<AuthenticationState>
{
	readonly AuthenticationStateProvider                 _service;
	readonly IHostEnvironmentAuthenticationStateProvider _environment;

	public AuthenticationStore(AuthenticationStateProvider service)
		: this(service, service.To<IHostEnvironmentAuthenticationStateProvider>()) {}

	public AuthenticationStore(AuthenticationStateProvider service,
	                           IHostEnvironmentAuthenticationStateProvider environment)
	{
		_service     = service;
		_environment = environment;
	}

	public ValueTask<AuthenticationState> Get() => _service.GetAuthenticationStateAsync().ToOperation();

	public void Execute(AuthenticationState parameter)
	{
		_environment.SetAuthenticationState(parameter.ToOperation().AsTask());
	}
}