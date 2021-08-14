using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authorization;
using System;

namespace DragonSpark.Application.Security.Identity.Claims.Policy
{
	public class AddPolicyConfiguration : ICommand<AuthorizationOptions>
	{
		readonly string                             _name;
		readonly Action<AuthorizationPolicyBuilder> _policy;

		protected AddPolicyConfiguration(string name, ICommand<AuthorizationPolicyBuilder> policy)
			: this(name, policy.Execute) {}

		protected AddPolicyConfiguration(string name, Action<AuthorizationPolicyBuilder> policy)
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