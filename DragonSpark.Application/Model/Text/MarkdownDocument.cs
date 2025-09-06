using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Model.Text;

public abstract class MarkdownDocument : Alteration<string>
{
	protected MarkdownDocument(string template) : this(new SmartFormat(template), Markdownify.Default) {}

	protected MarkdownDocument(IAlteration<string> format, IAlteration<string> content)
		: base(content.Then().Select(format)) {}
}