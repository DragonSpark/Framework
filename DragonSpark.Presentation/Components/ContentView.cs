using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DragonSpark.Presentation.Components
{
	public sealed class ContentView : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Source?.HasValue ?? false)
			{
				builder.AddContent(1, ChildContent);
			}
			else
			{
				builder.AddContent(2, Loading);
			}
		}

		[Parameter]
		public ISource Source { get; set; }

		[Parameter]
		public RenderFragment ChildContent { get; set; }

		[Parameter]
		public RenderFragment Loading { get; set; } = x => x.AddContent(2, "Loading, please wait.");
	}
}