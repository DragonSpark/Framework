using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Policy;

public sealed class RequireClaimValue : ICommand<AuthorizationPolicyBuilder>
{
	readonly string _type;
	readonly string _value;

	public RequireClaimValue(string type, string value)
	{
		_type  = type;
		_value = value;
	}

	public void Execute(AuthorizationPolicyBuilder parameter)
	{
		parameter.RequireClaim(_type, _value);
	}
}