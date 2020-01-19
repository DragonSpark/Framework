using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Elements {
	public sealed class Heading : ElementBase
	{
		public Heading() : base("h") {}

		protected override string Name(string name) => $"{name}{((int)Size).ToString()}";

		[Parameter]
		public HeadingSize Size { get; set; } = HeadingSize.Highest;
	}
}