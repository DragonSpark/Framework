using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Templates;

public sealed class MinimalLoadingTemplate : DragonSpark.Model.Results.Instance<RenderFragment>
{
	public static MinimalLoadingTemplate Default { get; } = new();

	MinimalLoadingTemplate() : base(x => x.AddContent(0, "Loading...")) {}
}