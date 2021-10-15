using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authorization;
using System;

namespace DragonSpark.Application.Compose;

sealed class ConfigurePolicies : ICommand<AuthorizationOptions>
{
	readonly Array<Action<AuthorizationOptions>> _policies;

	public ConfigurePolicies(params Action<AuthorizationOptions>[] policies) => _policies = policies;

	public void Execute(AuthorizationOptions parameter)
	{
		var policies = _policies.Open();
		var length   = policies.Length;
		for (var i = 0; i < length; i++)
		{
			_policies[i](parameter);
		}
	}
}