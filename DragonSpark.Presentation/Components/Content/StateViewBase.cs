using DragonSpark.Model.Results;
using DragonSpark.Presentation.Environment.Browser;
using DragonSpark.Runtime.Activation;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content;

public class StateViewBase<T> : Templates.ActiveContentTemplateComponentBase<T>
{
	[Parameter]
	public required IClientVariable<T> Definition { get; set; }

	[Parameter]
	public IResult<T> New { get; set; } = New<T>.Default;
}

public readonly record struct SaveStateViewContext<T>(T Subject, EventCallback Save);