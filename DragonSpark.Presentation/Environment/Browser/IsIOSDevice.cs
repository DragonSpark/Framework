using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class IsIOSDevice : ICondition<HttpRequest>
{
	public static IsIOSDevice Default { get; } = new();

	IsIOSDevice() : this("iPhone;", "iPad;") {}

	readonly Array<string> _candidates;

	public IsIOSDevice(params string[] candidates) => _candidates = candidates;

	public bool Get(HttpRequest parameter)
	{
		var agent = parameter.Headers.UserAgent.ToString();
		for (byte i = 0; i < _candidates.Length; i++)
		{
			if (agent.Contains(_candidates[i]))
			{
				return true;
			}
		}

		return false;
	}
}