using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DragonSpark.Presentation.Components
{
	public sealed class ContextView<TValue> : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Source?.HasValue ?? false)
			{
				builder.AddContent(1, ChildContent(Source.Value));
			}
			else
			{
				builder.AddContent(2, Loading);
			}
		}

		[Parameter]
		public ISource<TValue> Source { get; set; }

		[Parameter]
		public RenderFragment<TValue> ChildContent { get; set; }

		[Parameter]
		public RenderFragment Loading { get; set; } = x => x.AddContent(2, "Loading, please wait.");
	}
}