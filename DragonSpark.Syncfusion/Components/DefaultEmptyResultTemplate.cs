using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class DefaultEmptyResultTemplate : Instance<RenderFragment>
{
	public static DefaultEmptyResultTemplate Default { get; } = new();

	DefaultEmptyResultTemplate() : base(x => x.AddContent(0, "No records found for query")) {}
}