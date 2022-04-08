using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation;
using DragonSpark.Presentation.Components.Content.Templates;
using DragonSpark.Presentation.Components.State;
using DragonSpark.Syncfusion.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Components;

public class DataQueryComponent : DragonSpark.Presentation.Components.ComponentBase
{
	[Parameter]
	public virtual IDataRequest Content
	{
		get => _content;
		set
		{
			if (_content != value)
			{
				_content = value;
				Input = null;
			}
		}
	}	IDataRequest _content = default!;

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
	IActivityReceiver Receiver { get; set; } = default!;

	Await<DataManagerRequest, object>? Input { get; set; }

	protected override void OnParametersSet()
	{
		Input ??= _content.Then()
		                  .Then()
		                  .UpdateActivity(Receiver)
		                  .Handle(Exceptions, ReportedType ?? GetType(), EmptyDataResult.Default);
		base.OnParametersSet();
	}

	protected async Task OnRequest(DataRequestResult parameter)
	{
		var data = await Input!(parameter.Request);
		parameter.Execute(data);
	}
}