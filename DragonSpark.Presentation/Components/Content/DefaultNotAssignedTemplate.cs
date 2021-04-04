
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class DefaultNotAssignedTemplate : DragonSpark.Model.Results.Instance<RenderFragment>
	{
		public static DefaultNotAssignedTemplate Default { get; } = new ();

		DefaultNotAssignedTemplate() : base(x => x.AddContent(0, "This view's required information does not exist.")) {}
	}
}