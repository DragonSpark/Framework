using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Elements
{
	public sealed class Heading : ElementBase
	{
		public Heading() : base("h") {}

		[Parameter]
		public HeadingSize Size { get; set; } = HeadingSize.Highest;

		protected override string Name(string name) => $"{name}{((int)Size).ToString()}";
	}
}