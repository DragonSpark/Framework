using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Templates;

public sealed class DefaultEmptySequenceTemplate : DragonSpark.Model.Results.Instance<RenderFragment>
{
	public static DefaultEmptySequenceTemplate Default { get; } = new();

	DefaultEmptySequenceTemplate() : base(x => x.AddContent(0, "No elements found.")) {}
}