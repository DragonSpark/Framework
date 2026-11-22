using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Templates;

public sealed class DefaultExceptionTemplate : DragonSpark.Model.Results.Instance<RenderFragment<Exception>>
{
	public static DefaultExceptionTemplate Default { get; } = new();

	DefaultExceptionTemplate() : base(_ => x => x.AddContent(0, "Unfortunately, there was a problem loading this view.  You may need to refresh the page.")) {}
}