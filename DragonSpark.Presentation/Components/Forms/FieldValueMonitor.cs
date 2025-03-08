using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class FieldValueMonitor<T> : FieldNameMonitorBase
{
	[Parameter]
	public EventCallback<T> Changed { get; set; }

	protected override async Task OnUpdate()
	{
		var value = Identifier.GetValue<T>();
		await Changed.Invoke(value);
		await base.OnUpdate().Off();
	}
}