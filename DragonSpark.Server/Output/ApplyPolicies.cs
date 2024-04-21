using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output;

sealed class ApplyPolicies : ICommand<OutputCacheOptions>
{
	readonly Array<IOutputsPolicy> _policies;

	public ApplyPolicies(IOutputsPolicy[] policies) => _policies = policies;

	public void Execute(OutputCacheOptions parameter)
	{
		foreach (var policy in _policies)
		{
			parameter.AddPolicy(policy.Get(), policy);
		}
	}
}