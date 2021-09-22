
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class DefaultNotFoundTemplate : DragonSpark.Model.Results.Instance<RenderFragment>
	{
		public static DefaultNotFoundTemplate Default { get; } = new ();

		DefaultNotFoundTemplate() : base(x => x.AddContent(0, "This view's required information does not exist.")) {}
	}
}