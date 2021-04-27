using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class StructureContentView<TContent> : ComponentBase where TContent : struct
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Content.HasValue && ChildContent != null)
			{
				builder.AddContent(1, ChildContent(Content.Value));

			}
			else
			{
				builder.AddContent(2, NotFoundTemplate);
			}
		}

		[Parameter]
		public TContent? Content { get; set; }

		[Parameter]
		public RenderFragment<TContent>? ChildContent { get; set; }

		[Parameter]
		public RenderFragment NotFoundTemplate { get; set; } = x => x.AddContent(2, "Not found.");
	}
}