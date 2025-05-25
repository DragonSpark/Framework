using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

sealed class SignOutAwareAddExternalSignin : IAddExternalSignin
{
	readonly IAddExternalSignin _previous;
	readonly SignOutScheme      _clear;

	public SignOutAwareAddExternalSignin(IAddExternalSignin previous, SignOutScheme clear)
	{
		_previous = previous;
		_clear    = clear;
	}

	public async ValueTask<IdentityResult?> Get(Stop<ClaimsPrincipal> parameter)
	{
		var result = await _previous.Off(parameter);
		if (result?.Succeeded ?? false)
		{
			await _clear.Off();
		}

		return result;
	}
}