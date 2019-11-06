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

	public enum HeadingSize
	{
		Highest = 1, Higher = 2, High = 3, Low = 4, Lower = 5, Lowest = 6
	}

	public sealed class Heading : ElementBase
	{
		public Heading() : base("h") {}

		protected override string Name(string name) => $"{name}{((int)Size).ToString()}";

		[Parameter]
		public HeadingSize Size { get; set; } = HeadingSize.Highest;
	}

	public sealed class Paragraph : ElementBase
	{
		public Paragraph() : base("p") {}
	}

	public sealed class Container : ElementBase
	{
		public Container() : base("div") {}
	}
}
