using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Content.Templates;
using DragonSpark.Presentation.Components.State;
using DragonSpark.SyncfusionRendering.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Components;

public abstract class DataQueryComponent : DragonSpark.Presentation.Components.ComponentBase
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
	}	IDataRequest _content = default!;

	protected virtual void OnContentChanged(Await<DataManagerRequest, object>? parameter){}

	[Parameter]
	public Type? ReportedType { get; set; }

	[Parameter]
	public RenderFragment LoadingTemplate { get; set; } = DefaultLoadingTemplate.Default;

	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	[Parameter]
	public bool AllowPaging { get; set; } = true;

	[Parameter]
	public bool AllowFiltering { get; set; } = true;

	[Parameter]
	public bool AllowSorting { get; set; } = true;

	[Parameter]
	public string CssClass { get; set; } = string.Empty;

	[Parameter]
	public ushort PageSize { get; set; } = 10;

	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object> AdditionalAttributes { get; set; } = default!;

	[CascadingParameter]
	protected IActivityReceiver Receiver { get; set; } = default!;

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