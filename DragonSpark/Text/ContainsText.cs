using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Text;

public class ContainsText : ICondition<string>
{
	readonly string _text;

	public ContainsText(string text) => _text = text;

	public bool Get(string parameter) => parameter.Contains(_text);
}