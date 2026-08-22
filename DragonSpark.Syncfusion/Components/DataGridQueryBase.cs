using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.SyncfusionRendering.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace DragonSpark.SyncfusionRendering.Components;

public abstract class DataGridQueryBase<T> : DataGridBase<T>
{
	protected readonly Model.Results.Switch     _active = true;
	IStopAware<DataManagerRequest, DataResult>? _factory;

	[Inject]
	public required IDataRequests Requests { get; set; }

	protected abstract IDataRequest GetRequest();
	protected virtual IStopAware<DataManagerRequest, DataResult> ComposeFactory()
		=> Requests.Get(new(this, _identity, _active, GetRequest()));

	protected void RequestNewFactory()
	{
		_factory = null;
	}

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var previous = _identity;
		await base.SetParametersAsync(parameters).Off();
		if (_identity != previous)
		{
			RequestNewFactory();
		}

		_factory ??= ComposeFactory();
	}


	protected virtual async Task OnRequest(DataRequestResult parameter)
	{
		var data = await _factory.Verify().Off(new(parameter.Request, Stop));
		parameter.Execute(data);
	}

}