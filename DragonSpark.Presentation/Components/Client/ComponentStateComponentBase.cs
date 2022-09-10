using DragonSpark.Presentation.Environment.Browser;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Client;

public class ComponentStateComponentBase : Microsoft.AspNetCore.Components.ComponentBase
{
	[Parameter]
	public Microsoft.AspNetCore.Components.ComponentBase Owner { get; set; } = default!;

	[Parameter]
	public string? ProvidedKey { get; set; }

	[Parameter]
	public string Qualifier { get; set; } = default!;
}

public class ComponentStateComponentBase<T> : ComponentStateComponentBase
{
	[Parameter]
	public RenderFragment<IClientVariable<T>> ChildContent { get; set; } = default!;
}