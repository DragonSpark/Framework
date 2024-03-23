using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;

namespace DragonSpark.Text;

[UsedImplicitly]
public class StartsWithText : ICondition<string>
{
	readonly string _text;

	protected StartsWithText(string text) => _text = text;

	public bool Get(string parameter) => parameter.StartsWith(_text);
}