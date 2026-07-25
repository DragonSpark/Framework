using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.State;

public class StateAwareComponent : ComponentBase
{
	[Parameter]
	public EventCallback Initialized { get; set; }

	protected override Task OnAfterRenderAsync(bool firstRender)
		=> firstRender ? Initialized.Invoke() : base.OnAfterRenderAsync(firstRender);
}