using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Templates;

public sealed class PromptedExceptionTemplate : DragonSpark.Model.Results.Instance<RenderFragment<Exception>>
{
	public static PromptedExceptionTemplate Default { get; } = new();

	PromptedExceptionTemplate()
		: base(_ => x => x.AddContent(0, "There was a problem loading this view.  You may need to refresh the page.")) {}
}