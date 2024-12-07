using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.Security.Identity.Claims.Policy;

public class AddPolicyInstance : ICommand<AuthorizationOptions>
{
	readonly string              _name;
	readonly AuthorizationPolicy _policy;

	protected AddPolicyInstance(string name, params IAuthorizationRequirement[] requirements)
		: this(name, new AuthorizationPolicyBuilder().AddRequirements(requirements).Build()) {}

	protected AddPolicyInstance(string name, AuthorizationPolicy policy)
	{
		_name   = name;
		_policy = policy;
	}

	public void Execute(AuthorizationOptions parameter)
	{
		parameter.AddPolicy(_name, _policy);
	}
}