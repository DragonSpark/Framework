using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components
{
	public sealed class DefaultExceptionTemplate : DragonSpark.Model.Results.Instance<RenderFragment>
	{
		public static DefaultExceptionTemplate Default { get; } = new DefaultExceptionTemplate();

		DefaultExceptionTemplate() : base(x => x.AddContent(0, "There was a problem loading this view.")) {}
	}
}