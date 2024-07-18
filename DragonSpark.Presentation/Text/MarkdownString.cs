using DragonSpark.Application.Model.Text;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Text;

sealed class MarkdownString : Select<string, MarkupString>
{
	public static MarkdownString Default { get; } = new();

	MarkdownString() : base(Markdownify.Default.Then().Select(x => (MarkupString)x)) {}
}