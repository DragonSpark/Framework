using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class EmptyContentTemplate : DragonSpark.Model.Results.Instance<RenderFragment>
	{
		public static EmptyContentTemplate Default { get; } = new ();

		EmptyContentTemplate() : base(_ => {}) {}
	}
}