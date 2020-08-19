using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace DragonSpark.Presentation.Components.Forms
{
	public class EditTemplate : ComponentBase
	{
		[CascadingParameter]
		EditContext Context { get; set; } = default!;

		[Parameter]
		public RenderFragment<EditContext> ChildContent { get; set; } = default!;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddContent(1, ChildContent(Context));
		}
	}
}
