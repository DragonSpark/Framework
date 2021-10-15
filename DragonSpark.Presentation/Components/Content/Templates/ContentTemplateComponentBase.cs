using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Templates;

public class ContentTemplateComponentBase<T> : ComponentBase
{
	[Parameter]
	public virtual RenderFragment<T> ChildContent { get; set; } = EmptyContentTemplate.Default.Get().Accept;

	[Parameter]
	public virtual RenderFragment NotFoundTemplate { get; set; } = DefaultNotFoundTemplate.Default;
}