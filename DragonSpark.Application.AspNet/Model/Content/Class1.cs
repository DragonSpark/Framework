using DragonSpark.Application.Model.Text;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Model.Content;

class Class1;

// TODO

sealed class AsMarkdown : ISelect<string?, MarkupString>
{
	public static AsMarkdown Default { get; } = new();

	AsMarkdown() {}

	public MarkupString Get(string? parameter)
		=> new(!parameter.IsNullOrEmpty() ? Markdownify.Default.Get(parameter.Verify()) : string.Empty);
}