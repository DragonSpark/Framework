using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Markdig;
using SmartFormat;
using System.Text.RegularExpressions;

namespace DragonSpark.Application.Model.Text;

// TODO

public abstract class MarkdownDocument : IAlteration<string>
{
	readonly string              _template;
	readonly IAlteration<string> _content;

	protected MarkdownDocument(string template) : this(template, Markdownify.Default) {}

	protected MarkdownDocument(string template, IAlteration<string> content)
	{
		_template = template;
		_content  = content;
	}

	public string Get(string parameter) => _template.FormatSmart(_content.Get(parameter));
}


public sealed class Markdownify : IAlteration<string>
{
	public static Markdownify Default { get; } = new();

	Markdownify() : this(ReplaceLineBreaks.Default.Then().Select(ReplaceTabs.Default).Out()) {}

	readonly IAlteration<string> _replace;

	public Markdownify(IAlteration<string> replace) => _replace = replace;

	public string Get(string parameter)
	{
		var content = _replace.Get(parameter);
		var result  = Markdown.ToHtml(content);
		return result;
	}
}

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

sealed class ReplaceTabs : Replace
{
	public static ReplaceTabs Default { get; } = new();

	ReplaceTabs() : base(new(@"\t"), "&nbsp;&nbsp;&nbsp;&nbsp;") {}
}

sealed class ReplaceLineBreaks : Replace
{
	public static ReplaceLineBreaks Default { get; } = new();

	ReplaceLineBreaks() : base(new(@"([^\s\\])\r?\n"), "$1<br />") {}
}