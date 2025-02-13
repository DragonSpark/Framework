using System;
using System.Security.Claims;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Identity.Claims;

public class RequiredClaim : IRequiredClaim
{
	readonly string                        _claim;
	readonly Func<ClaimsPrincipal, string> _message;

	protected RequiredClaim(string claim)
		: this(claim,
		       x => $"Content not found for claim '{claim}' {x.Identity?.AuthenticationType}-{x.Identity?.Name}.") {}

	protected RequiredClaim(string claim, Func<ClaimsPrincipal, string> message)
	{
		_claim   = claim;
		_message = message;
	}

	public string Get(ClaimsPrincipal parameter)
		=> parameter.FindFirstValue(_claim) ?? throw new InvalidOperationException(_message(parameter));
}

public class RequiredClaim<T> : Select<ClaimsPrincipal, T>
{
	protected RequiredClaim(string claim, Func<string, T> parse) : this(new Implementation(claim), parse) {}

	protected RequiredClaim(IRequiredClaim claim, Func<string, T> parse) : base(claim.Then().Select(parse)) {}

	sealed class Implementation : RequiredClaim
	{
		public Implementation(string claim) : base(claim) {}
	}
}