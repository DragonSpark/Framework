using DragonSpark.Compose;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class ValidateUser : IValidateUser
{
	readonly IValidationServices _validation;
	readonly SignOutUser         _exit;

	public ValidateUser(IValidationServices validation, SignOutUser exit)
	{
		_validation = validation;
		_exit       = exit;
	}

	public async ValueTask<bool> Get(ClaimsPrincipal parameter)
	{
		var result = await _validation.Get(parameter).On();
		if (!result)
		{
			_exit.Execute();
		}

		return result;
	}
}