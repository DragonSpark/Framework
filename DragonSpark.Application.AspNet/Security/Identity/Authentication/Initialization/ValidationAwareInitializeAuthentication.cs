using DragonSpark.Compose;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class ValidationAwareInitializeAuthentication : IInitializeAuthentication
{
	readonly IInitializeAuthentication _previous;
	readonly ValidateUser              _validate;

	public ValidationAwareInitializeAuthentication(IInitializeAuthentication previous, ValidateUser validate)
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