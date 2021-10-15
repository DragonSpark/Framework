using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

sealed class StateAwareAddExternalSignin : IAddExternalSignin
{
	readonly IAddExternalSignin        _previous;
	readonly IClearAuthenticationState _clear;

	public StateAwareAddExternalSignin(IAddExternalSignin previous, IClearAuthenticationState clear)
	{
		_previous = previous;
		_clear    = clear;
	}

	public async ValueTask<IdentityResult?> Get(ClaimsPrincipal parameter)
	{
		var result = await _previous.Await(parameter);
		if (result?.Succeeded ?? false)
		{
			_clear.Execute(parameter);
		}

		return result;
	}
}