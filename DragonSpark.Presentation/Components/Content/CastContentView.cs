using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class CastContentView<TContent> : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Content is null)
			{
				builder.AddContent(1, NotFoundTemplate);
			}
			else if (ChildContent != null)
			{
				if (Content is TContent content)
				{
					builder.AddContent(2, ChildContent(content));
				}
				else
				{
					builder.AddContent(3, NotCompatibleTemplate);

				}
			}
		}

		[Parameter, UsedImplicitly]
		public object? Content { get; set; }

		[Parameter, UsedImplicitly]
		public RenderFragment<TContent>? ChildContent { get; set; }

		[Parameter]
		public RenderFragment NotFoundTemplate { get; set; } = x => x.AddContent(2, "Not found.");

		[Parameter]
		public RenderFragment NotCompatibleTemplate { get; set; } = x => x.AddContent(2, "The provided content is not compatible with this view.");
	}
}