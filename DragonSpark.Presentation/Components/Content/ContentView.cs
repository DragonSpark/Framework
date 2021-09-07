using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class ContentView<TValue> : ComponentBase where TValue : class
	{
		protected override void OnParametersSet()
		{
			base.OnParametersSet();
			Fragment ??= Content != null ? ChildContent(Content) : NotFoundTemplate;
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddContent(0, Fragment);
		}

		RenderFragment? Fragment { get; set; }

		[Parameter]
		public TValue? Content
		{
			get => _content;
			set
			{
				if (_content != value)
				{
					_content = value;
					Fragment = null;
				}
			}
		}

		TValue? _content;

		[Parameter]
		public RenderFragment<TValue> ChildContent { get; set; } = default!;

		[Parameter]
		public RenderFragment NotFoundTemplate { get; set; } = x => x.AddContent(2, "Not found.");
	}
}