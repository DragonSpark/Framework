using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

sealed class IsPlatformAgent : ICondition<HttpRequest>
{
	public static IsPlatformAgent Default { get; } = new();

	IsPlatformAgent() : this(PlatformAgents.Default) {}

	readonly Array<string>    _agents;
	readonly StringComparison _comparison;

	public IsPlatformAgent(Array<string> agents,
	                       StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
	{
		_agents     = agents;
		_comparison = comparison;
	}

	public bool Get(HttpRequest parameter)
	{
		if (parameter.Headers.UserAgent.ToString().NullIfEmpty() is {} agent)
		{
			foreach (var crawler in _agents)
			{
				if (agent.Contains(crawler, _comparison))
				{
					return true;
				}
			}
		}

		return false;
	}
}