using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

sealed class SignOutAwareAddExternalSignin : IAddExternalSignin
{
	readonly IAddExternalSignin _previous;
	readonly SignOutScheme      _clear;

	public SignOutAwareAddExternalSignin(IAddExternalSignin previous, SignOutScheme clear)
	{
		_previous = previous;
		_clear    = clear;
	}

	public async ValueTask<IdentityResult?> Get(ClaimsPrincipal parameter)
	{
		var result = await _previous.Await(parameter);
		if (result?.Succeeded ?? false)
		{
			await _clear.Await();
		}

		return result;
	}
}