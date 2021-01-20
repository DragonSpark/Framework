using DragonSpark.Reflection.Members;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Dialogs
{
	sealed class RenderFragments : FieldAccessor<Microsoft.AspNetCore.Components.ComponentBase, RenderFragment>
	{
		public static RenderFragments Default { get; } = new();

		RenderFragments() : base("_renderFragment") {}
	}
}