using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Markdig;

namespace DragonSpark.Application.Model.Text;

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