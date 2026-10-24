using DragonSpark.Compose;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class ValidationAwareInitializeAuthentication : IInitializeAuthentication
{
	readonly ValidateUser              _validate;
	readonly IInitializeAuthentication _previous;

	public ValidationAwareInitializeAuthentication(ValidateUser validate, IInitializeAuthentication previous)
	{
		_previous = previous;
		_validate = validate;
	}

	public async ValueTask Get(ClaimsPrincipal parameter)
	{
		if (await _validate.On(parameter))
		{
			await _previous.Off(parameter);
		}
	}
}