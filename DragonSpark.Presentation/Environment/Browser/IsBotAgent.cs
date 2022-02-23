using DeviceDetectorNET.Parser;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class IsBotAgent : ICondition<string>
{
	public static IsBotAgent Default { get; } = new();

	IsBotAgent() {}

	public bool Get(string parameter)
	{
		var sut = new BotParser { DiscardDetails = true };
		sut.SetUserAgent(parameter);
		var result = sut.Parse().Success;
		return result;
	}
}