using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.Communication;

sealed class AssignSameSite : ICommand<AppendCookieContext>
{
	public static AssignSameSite Default { get; } = new();

	AssignSameSite() : this(AgentShouldOmit.Default) {}

	readonly ICondition<string> _agent;

	public AssignSameSite(ICondition<string> agent) => _agent = agent;

	public void Execute(AppendCookieContext parameter)
	{
		Execute(parameter.Context, parameter.CookieOptions);
	}

	public void Execute(DeleteCookieContext parameter)
	{
		Execute(parameter.Context, parameter.CookieOptions);
	}

	void Execute(HttpContext context, CookieOptions options)
	{
		switch (options.SameSite)
		{
			case SameSiteMode.None:
				if (_agent.Get(context.Request.Headers.UserAgent.ToString()))
				{
					options.SameSite = SameSiteMode.Unspecified;
				}

				break;
		}
	}
}