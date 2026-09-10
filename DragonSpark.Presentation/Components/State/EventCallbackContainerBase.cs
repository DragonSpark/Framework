using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.State;

public abstract class EventCallbackContainerBase : InteractiveComponentBase
{
	[Parameter, EditorRequired]
	public required object Owner { get; set; }

	[Parameter]
	public required bool PreserveNotificationOwner { get; set; } = true;

	[Parameter]
	public required string StopMessage { get; set; } = "An operation is active.  Press Cancel below to stop it.";

	[Parameter]
	public required IStopHandle? StopHandle { get; set; }

	[Parameter]
	public required IOperation? Stopped { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = HasChanged(parameters);
		await base.SetParametersAsync(parameters).On();
		if (changed)
		{
			await Changed().Off();
		}
	}

	protected abstract Task Changed();

	protected virtual bool HasChanged(ParameterView parameters)
		=> parameters.DidParameterChange(nameof(Owner), Owner)
		   || parameters.DidParameterChange(nameof(StopHandle), StopHandle)
		   || parameters.DidParameterChange(nameof(StopMessage), StopMessage)
		   || parameters.DidParameterChange(nameof(Stopped), Stopped);

}
