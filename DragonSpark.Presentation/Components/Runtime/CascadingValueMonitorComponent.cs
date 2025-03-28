using DragonSpark.Compose;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Runtime;

public sealed class CascadingValueMonitorComponent<T> : InteractiveComponentBase
{
	[CascadingParameter]
	T? Subject { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = Subject is not null && parameters.DidParameterChange(nameof(Subject), Subject);

		await base.SetParametersAsync(parameters).On();

		if (changed)
		{
			await Updated.Invoke().Off();
		}
	}
}