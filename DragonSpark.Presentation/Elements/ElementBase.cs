using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DragonSpark.Presentation.Elements
{
	public abstract class ElementBase : ComponentBase
	{
		readonly string _name;

		protected ElementBase(string name) => _name = name;

		[Parameter]
		public string StyleType { get; set; }

		[Parameter]
		public string InlineStyle { get; set; }

		[Parameter]
		public RenderFragment ChildContent { get; set; }

		protected virtual string Name(string name) => name;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, Name(_name));
			builder.AddAttribute(1, "class", StyleType);
			builder.AddAttribute(2, "style", InlineStyle);
			builder.AddContent(3, ChildContent);
			builder.CloseElement();
		}
	}
}
