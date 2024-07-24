using DragonSpark.Model.Selection.Alterations;
using SmartFormat;

namespace DragonSpark.Application.Model.Text;

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