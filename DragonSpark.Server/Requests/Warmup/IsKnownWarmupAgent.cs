using DragonSpark.Application.Communication;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Requests.Warmup;

sealed class IsKnownWarmupAgent : ICondition<IHeaderDictionary>
{
	public static IsKnownWarmupAgent Default { get; } = new();

	IsKnownWarmupAgent() : this(UserAgentHeader.Default, KnownUserAgents.Default) { }

	readonly IHeader       _header;
	readonly Array<string> _agents;

	public IsKnownWarmupAgent(IHeader header, Array<string> agents)
	{
		_header = header;
		_agents = agents;
	}

	public bool Get(IHeaderDictionary parameter)
	{
		var agent  = _header.Get(parameter);
		var result = agent is not null && _agents.AsValueEnumerable().Contains(agent);
		return result;
	}
}