using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims;

public class RequiredClaim : IRequiredClaim
{
	readonly string                        _claim;
	readonly Func<ClaimsPrincipal, string> _message;

	protected RequiredClaim(string claim)
	: this(claim, x => $"Content not found for claim '{claim}' {x.Identity?.AuthenticationType}-{x.Identity?.Name}.") {}
	protected RequiredClaim(string claim, Func<ClaimsPrincipal, string> message)
	{
		_claim        = claim;
		_message = message;
	}
	public string Get(ClaimsPrincipal parameter)
		=> parameter.FindFirstValue(_claim) ?? throw new InvalidOperationException(_message(parameter));
}