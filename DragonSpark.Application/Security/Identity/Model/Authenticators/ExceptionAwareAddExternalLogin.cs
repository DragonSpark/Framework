using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

sealed class ExceptionAwareAddExternalLogin : IAddExternalSignin
{
	readonly IAddExternalSignin _previous;
	readonly IExceptionLogger   _logger;

	public ExceptionAwareAddExternalLogin(IAddExternalSignin previous, IExceptionLogger logger)
	{
		_previous = previous;
		_logger   = logger;
	}

	public async ValueTask<IdentityResult?> Get(ClaimsPrincipal parameter)
	{
		try
		{
			return await _previous.Await(parameter);
		}
		catch (Exception e)
		{
			await _logger.Await(new (GetType(), e));
			return IdentityResult.Failed(new IdentityError
			{
				Description =
					"A unexpected problem occurred while adding this external identity to your account and has been recorded for system administrators to review."
			});
		}
	}
}