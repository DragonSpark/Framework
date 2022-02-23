using DeviceDetectorNET.Parser;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class IsBot : ICondition<HttpRequest>
{
	public static IsBot Default { get; } = new();

	IsBot() : this(UserAgent.Default) {}

	readonly IFormatter<HttpRequest> _agent;

	public IsBot(IFormatter<HttpRequest> agent) => _agent = agent;

	public bool Get(HttpRequest parameter)
	{
		var sut = new BotParser { DiscardDetails = true };
		sut.SetUserAgent(_agent.Get(parameter));
		var result = sut.Parse().Success;
		return result;
	}
}