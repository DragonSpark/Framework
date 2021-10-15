using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Templates;

public class ActiveContentTemplateComponentBase<T> : ContentTemplateComponentBase<T>
{
	[Parameter]
	public virtual RenderFragment LoadingTemplate { get; set; } = DefaultLoadingTemplate.Default;

	[Parameter]
	public virtual RenderFragment ExceptionTemplate { get; set; } = DefaultExceptionTemplate.Default;

}