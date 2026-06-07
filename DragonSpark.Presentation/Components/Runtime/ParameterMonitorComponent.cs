using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Runtime;

public sealed class ParameterMonitorComponent<T> : ComponentBase
{
	[Parameter, EditorRequired]
	public T? Subject { get; set; }

	[Parameter]
	public required EventCallback<T?> SubjectChanged { get; set; }
	
	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Subject), Subject);

		await base.SetParametersAsync(parameters).On();

		if (changed)
		{
			await SubjectChanged.Off(Subject);
		}
	}
}