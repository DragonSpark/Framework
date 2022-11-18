using DragonSpark.Model.Selection;
using Markdig;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace DragonSpark.Presentation.Text;

sealed class MarkDownify : ISelect<string, MarkupString>
{
	readonly Regex  _expression;
	readonly string _replace;
	public static MarkDownify Default { get; } = new();

	MarkDownify() : this(new Regex(@"([^\s\\])\r?\n\b"), "$1<br />") {}

	public MarkDownify(Regex expression, string replace)
	{
		_expression   = expression;
		_replace = replace;
	}

	public MarkupString Get(string parameter)
	{
		var content = _expression.Replace(parameter, _replace);
		var result  = (MarkupString)Markdown.ToHtml(content);
		return result;
	}
}