using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Templates
{
	public class ManyActiveContentTemplateComponentBase<T> : ActiveContentTemplateComponentBase<T>
	{
		[Parameter]
		public virtual RenderFragment EmptyElementsTemplate { get; set; } = DefaultEmptySequenceTemplate.Default;
	}
}