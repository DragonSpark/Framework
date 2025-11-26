using DragonSpark.Model.Selection.Alterations;
using System.Text.RegularExpressions;

namespace DragonSpark.Application.Model.Text;

public sealed class StripMarkup : IAlteration<string>
{
	public static StripMarkup Default { get; } = new();

	StripMarkup() : this(new("<[^>]*>")) {}

	readonly Regex _expression;

	public StripMarkup(Regex expression) => _expression = expression;

	public string Get(string parameter) => _expression.Replace(parameter, string.Empty);
}