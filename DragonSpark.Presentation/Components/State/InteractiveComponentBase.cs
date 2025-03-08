using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.State;

public class InteractiveComponentBase : ComponentBase
{
	[Parameter]
	public EventCallback Updated { get; set; }

	[CascadingParameter]
	protected IActivityReceiver Receiver { get; set; } = null!;

	protected override bool ShouldRender() => !Receiver.Active;
}