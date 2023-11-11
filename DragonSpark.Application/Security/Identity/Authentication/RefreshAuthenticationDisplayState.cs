using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Conditions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class RefreshAuthenticationDisplayState<T> : IDepending<ClaimsPrincipal> where T : IdentityUser
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

	public async ValueTask<bool> Get(ClaimsPrincipal parameter)
	{
		var previous = await _service.GetAuthenticationStateAsync();
		var number   = parameter.Number();
		if (number is not null)
		{
			_clear.Execute(number.Value);
		}
		var next = await _service.GetAuthenticationStateAsync();
		var result = previous.User.IsAuthenticated() != next.User.IsAuthenticated()
		             || previous.To<AuthenticationState<T>>().Profile?.SecurityStamp !=
		             next.To<AuthenticationState<T>>().Profile?.SecurityStamp;
		if (result)
		{
			_environment.SetAuthenticationState(next.ToOperation().AsTask());
		}
		return result;
	}
}