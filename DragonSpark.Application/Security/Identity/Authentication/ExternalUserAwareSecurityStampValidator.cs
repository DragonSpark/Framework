using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class ExternalUserAwareSecurityStampValidator : ISecurityStampValidator
{
	readonly ISecurityStampValidator     _previous;
	readonly ICondition<ClaimsPrincipal> _external;

	public ExternalUserAwareSecurityStampValidator(ISecurityStampValidator previous) 
		: this(previous, IsExternalPrincipal.Default) {}

	public ExternalUserAwareSecurityStampValidator(ISecurityStampValidator previous,
	                                               ICondition<ClaimsPrincipal> external)
	{
		_previous = previous;
		_external = external;
	}

	public async Task ValidateAsync(CookieValidatePrincipalContext context)
	{
		if (context.Principal is not null && _external.Get(context.Principal))
		{
			try
			{
				await _previous.ValidateAsync(context).Await();
			}
			catch (ArgumentException)
			{
				context.RejectPrincipal();
			}

			return;
		}

		await _previous.ValidateAsync(context).Await();
	}
}