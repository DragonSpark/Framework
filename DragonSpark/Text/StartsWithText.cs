using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Text;

public class StartsWithText : ICondition<string>
{
	readonly string _text;

	public StartsWithText(string text) => _text = text;

	public bool Get(string parameter) => parameter.StartsWith(_text);
}