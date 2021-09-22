using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class NotFoundTemplate : DragonSpark.Model.Results.Instance<RenderFragment>
	{
		public static NotFoundTemplate Default { get; } = new ();

		NotFoundTemplate() : base(x => x.AddContent(0, "Not found.")) {}
	}
}