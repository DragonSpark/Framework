using DragonSpark.Text;
using Humanizer;

namespace DragonSpark.Application.Model.Text;

public sealed class HumanizedTitleCase : IFormatter<string>
{
	public static HumanizedTitleCase Default { get; } = new();

	HumanizedTitleCase() : this(TitleCase.Default) {}

	readonly IFormatter<string> _previous;

	public HumanizedTitleCase(IFormatter<string> previous) => _previous = previous;

	public string Get(string parameter) => _previous.Get(parameter.Humanize());
}