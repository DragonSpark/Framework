using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class ContentView<TValue> : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Content is null)
			{
				builder.AddContent(2, NotFoundTemplate);
			}
			else if (ChildContent != null)
			{
				builder.AddContent(1, ChildContent(Content));
			}
		}

		[Parameter, UsedImplicitly]
		public TValue? Content { get; set; }

		[Parameter, UsedImplicitly]
		public RenderFragment<TValue>? ChildContent { get; set; }

		[Parameter]
		public RenderFragment NotFoundTemplate { get; set; } = x => x.AddContent(2, "Not found.");
	}
}