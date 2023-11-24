using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection.Conditions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class RefreshAuthenticationDisplayState<T> : IDepending<ClaimsPrincipal> where T : IdentityUser
{
	readonly IClearAuthenticationState _clear;
	readonly AuthenticationStore       _store;

	public RefreshAuthenticationDisplayState(IClearAuthenticationState clear, AuthenticationStore store)
	{
		_clear = clear;
		_store = store;
	}

	public async ValueTask<bool> Get(ClaimsPrincipal parameter)
	{
		var previous = await _store.Await();
		var number   = parameter.Number();
		if (number is not null)
		{
			_clear.Execute(number.Value);
		}

		var next = await _store.Await();
		var result = previous.User.IsAuthenticated() != next.User.IsAuthenticated()
		             || previous.To<AuthenticationState<T>>().Profile?.SecurityStamp !=
		             next.To<AuthenticationState<T>>().Profile?.SecurityStamp;
		if (result)
		{
			_store.Execute(next);
		}

		return result;
	}
}

// TODO

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