using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Communication;

sealed class AgentShouldOmit : ICondition<string>
{
	public static AgentShouldOmit Default { get; } = new();

	AgentShouldOmit() {}

	public bool Get(string parameter)
		=> parameter.Contains("CPU iPhone OS ") || parameter.Contains("iPad; CPU OS ") ||
		   parameter.Contains("Macintosh; Intel Mac OS X ") &&
		   parameter.Contains("Version/") && parameter.Contains("Safari") || parameter.Contains("Chrome/5") ||
		   parameter.Contains("Chrome/6");
}