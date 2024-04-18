using DragonSpark.Presentation.Components.Content.Templates;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace DragonSpark.SyncfusionRendering.Components;

public class DataComponent : DragonSpark.Presentation.Components.ComponentBase
{
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
}