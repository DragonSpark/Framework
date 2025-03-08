using DragonSpark.Model.Operations;
using DragonSpark.SyncfusionRendering.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Components;

public abstract class DataQueryComponent : DataComponent
{
	[Parameter]
	public IDataRequest Content
	{
		get => _content;
		set
		{
			if (_content != value)
			{
				_content = value;
				OnContentChanged(Input);
				Input = null;
			}
		}
	}	IDataRequest _content = null!;

	protected virtual void OnContentChanged(Await<DataManagerRequest, object>? parameter){}

	Await<DataManagerRequest, object>? Input { get; set; }

	protected override void OnParametersSet()
	{
		Input ??= CreateInput();
		base.OnParametersSet();
	}

	protected abstract Await<DataManagerRequest, object> CreateInput();

	protected virtual async Task OnRequest(DataRequestResult parameter)
	{
		var data = await Input!(parameter.Request);
		parameter.Execute(data);
	}
}