using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Runtime;

public class RelayComponentBase<T> : ComponentBase
{
	[Parameter]
	public required bool Enabled { get; set; } = true;

	[Parameter]
	public required T Input { get; set; }

	[Parameter]
	public EventCallback<T> Changed { get; set; }
}