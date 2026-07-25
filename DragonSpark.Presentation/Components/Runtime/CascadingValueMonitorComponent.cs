using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.Runtime;

public sealed class CascadingValueMonitorComponent<T> : ComponentBase
{
	[CascadingParameter]
	T? Subject { get; set; }

	[Parameter]
	public required EventCallback<T?> SubjectChanged { get; set; }
	
	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = Subject is not null && parameters.DidParameterChange(nameof(Subject), Subject);

		await base.SetParametersAsync(parameters).On();

		if (changed)
		{
			await SubjectChanged.Off(Subject);
		}
	}
}