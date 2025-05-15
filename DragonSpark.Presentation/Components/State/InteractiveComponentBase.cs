using Microsoft.AspNetCore.Components;
using System.Threading;

namespace DragonSpark.Presentation.Components.State;

public class InteractiveComponentBase : ComponentBase
{
	[Parameter]
	public EventCallback Updated { get; set; }

	[CascadingParameter]
	public required IActivityReceiver Receiver { get; set; }

	[CascadingParameter]
	public required CancellationToken Cancel { get; set; } = CancellationToken.None;

	protected override bool ShouldRender() => !Receiver.Active;
}

public class InteractiveComponentBase<T> : InteractiveComponentBase
{
	[Parameter]
	public required T Input { get; set; }
}