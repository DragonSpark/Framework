using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.SyncfusionRendering.Queries;
using Microsoft.AspNetCore.Components;
using Radzen;
using Syncfusion.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Components;

public abstract class DataQueryComponent : DataComponent
{
	Await<Stop<DataManagerRequest>, object>? _input;

	[Parameter]
	public required IDataRequest Content { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Content), Content);
		await base.SetParametersAsync(parameters).Off();
		if (changed)
		{
			_input = CreateInput();
		}
	}

	protected abstract Await<Stop<DataManagerRequest>, object> CreateInput();

	protected virtual async Task OnRequest(DataRequestResult parameter)
	{
		var data = await _input.Verify()(new(parameter.Request, Stop));
		parameter.Execute(data);
	}
}