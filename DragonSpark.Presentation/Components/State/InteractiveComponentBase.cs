using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.State;

public class InteractiveComponentBase : ComponentBase
{
	[Parameter]
	public EventCallback Updated { get; set; }

	[CascadingParameter]
	public required IActivityReceiver Receiver { get; set; }

	protected override bool ShouldRender() => !Receiver.Active;
}

public class InteractiveComponentBase<T> : InteractiveComponentBase
{
	[Parameter]
	public required T Input { get; set; }
}