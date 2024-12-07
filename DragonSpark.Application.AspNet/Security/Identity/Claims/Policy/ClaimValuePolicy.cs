using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;
using System;

namespace DragonSpark.Application.Security.Identity.Claims.Policy;

public class ClaimValuePolicy : ICommand<AuthorizationOptions>
{
	readonly string                             _name;
	readonly Action<AuthorizationPolicyBuilder> _claim;

	protected ClaimValuePolicy(string name, string type, string value)
		: this(name, new RequireClaimValue(type, value).Execute) {}

	protected ClaimValuePolicy(string name, Action<AuthorizationPolicyBuilder> claim)
	{
		_name  = name;
		_claim = claim;
	}

	public void Execute(AuthorizationOptions parameter)
	{
		parameter.AddPolicy(_name, _claim);
	}
}