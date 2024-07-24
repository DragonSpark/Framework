using DragonSpark.Model.Selection.Alterations;
using System.Text.RegularExpressions;

namespace DragonSpark.Application.Model.Text;

public class Replace : IAlteration<string>
{
	readonly Regex  _expression;
	readonly string _replace;

	protected Replace(Regex expression, string replace)
	{
		_expression = expression;
		_replace    = replace;
	}
	public string Get(string parameter) => _expression.Replace(parameter, _replace);
}