using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;

namespace DragonSpark.Azure.Ai;

sealed class StylizedPrompt : IAlteration<string>
{
	public static StylizedPrompt Default { get; } = new();

	StylizedPrompt() : this(Style.Default) {}

	readonly IText _style;

	public StylizedPrompt(IText style) => _style = style;

	public string Get(string parameter)
		=> $"{parameter}, {_style.Get()}, very creative and unpredictable composition, maximum artistic freedom, high variation, avoid repeating previous compositions";
}