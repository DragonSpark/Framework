using DragonSpark.Text;
using System.Globalization;

namespace DragonSpark.Application.Model.Text;

public sealed class TitleCase : IFormatter<string>
{
	public static TitleCase Default { get; } = new();

	TitleCase() : this(CultureInfo.CurrentCulture.TextInfo) {}

	readonly TextInfo _text;

	public TitleCase(TextInfo text) => _text = text;

	public string Get(string parameter) => _text.ToTitleCase(parameter.Replace('‘', '\'').Replace('`', '\''));
}