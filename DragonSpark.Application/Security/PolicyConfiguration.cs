using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Application.Security
{
	public class PolicyConfiguration : ICommand<AuthorizationOptions>
	{
		readonly string              _name;
		readonly AuthorizationPolicy _policy;

		public PolicyConfiguration(string name, params IAuthorizationRequirement[] requirements)
			: this(name, new AuthorizationPolicyBuilder().AddRequirements(requirements).Build()) {}

		public PolicyConfiguration(string name, AuthorizationPolicy policy)
		{
			_name   = name;
			_policy = policy;
		}

		public void Execute(AuthorizationOptions parameter)
		{
			parameter.AddPolicy(_name, _policy);
		}
	}
}