using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components
{
	public sealed class DefaultLoadingTemplate : Model.Results.Instance<RenderFragment>
	{
		public static DefaultLoadingTemplate Default { get; } = new DefaultLoadingTemplate();

		DefaultLoadingTemplate() : base(x => x.AddContent(0, "Loading, please wait.")) {}
	}
}