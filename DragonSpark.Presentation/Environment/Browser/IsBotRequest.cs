﻿using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class IsBotRequest : ICondition<HttpRequest>
{
	public static IsBotRequest Default { get; } = new();

	IsBotRequest()
		: this(UserAgent.Default,
		       Start.A.Condition<string>()
		            .By.Calling(string.IsNullOrEmpty)
		            .Or(Start.A.Selection<string, string>(string.Intern)
		                     .Then()
		                     .Select(IsBotAgent.Default.Then())
		                     .Get()
		                     .ToConcurrentTable())
		            .Out()) {}

	readonly IFormatter<HttpRequest> _agent;
	readonly ICondition<string>      _bot;

	public IsBotRequest(IFormatter<HttpRequest> agent, ICondition<string> bot)
	{
		_agent = agent;
		_bot   = bot;
	}

	public bool Get(HttpRequest parameter)
	{
		var agent  = _agent.Get(parameter);
		var result = _bot.Get(agent);
		return result;
	}
}