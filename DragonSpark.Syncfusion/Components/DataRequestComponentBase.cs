using BlazorPro.BlazorSize;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Components.Content.Sequences;
using DragonSpark.SyncfusionRendering.Entities;
using DragonSpark.SyncfusionRendering.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;

namespace DragonSpark.SyncfusionRendering.Components;

public class DataRequestTemplateComponentBase
	: DragonSpark.Presentation.Components.Content.Templates.ManyActiveContentTemplateComponentBase<IDataRequest>
{
	[Parameter]
	public IReporter<IDataRequest>? Reporter { get; set; } = Queries.Reporter.Default;
}

public class DataGridTemplateComponentBase<T> : DataRequestTemplateComponentBase
{
	[Parameter]
	public Guid Id { get; set; }

	[Parameter]
	public string Qualifier { get; set; } = string.Empty;

	[Parameter]
	public ICondition<bool?> Results { get; set; } = HasResults.Default;

	[Parameter]
	public FilterType FilterType { get; set; } = FilterType.Excel;

	[Parameter]
	public RenderFragment Columns { get; set; } = null!;

	[Parameter]
	public RenderFragment? BodyHeaderTemplate { get; set; }

	[Parameter]
	public RenderFragment? BodyContentTemplate { get; set; }

	[Parameter]
	public RenderFragment? BodyFooterTemplate { get; set; }

	[Parameter]
	public RenderFragment<PagingRenderState<T>>? HeaderTemplate { get; set; }

	[Parameter]
	public RenderFragment<PagingRenderState<T>>? FooterTemplate { get; set; }

	[Parameter]
	public bool AllowSelection { get; set; }

	[Parameter]
	public bool AllowExport { get; set; }

	[Parameter]
	public bool AllowPaging { get; set; } = true;

	[Parameter]
	public bool AllowFiltering { get; set; } = true;

	[Parameter]
	public bool AllowSorting { get; set; } = true;

	[Parameter]
	public string Breakpoint { get; set; } = Breakpoints.SmallUp;

	[Parameter]
	public string CssClass { get; set; } = string.Empty;

	[Parameter]
	public ICollection<string>? DesktopToolbar { get; set; }

	[Parameter]
	public ICollection<string>? MobileToolbar { get; set; } = DefaultToolbar.Default;

	[Parameter]
	public ushort PageSize { get; set; } = 10;

	[Parameter]
	public EventCallback<Allow<T>> Editing { get; set; }

	[Parameter]
	public EventCallback<T> Created { get; set; }

	[Parameter]
	public EventCallback<Updated<T>> Updated { get; set; }

	[Parameter]
	public Type? ReportedType { get; set; }

	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object> AdditionalAttributes { get; set; } = null!;
}

/*public class DataRequestComponentBase<T> : DataGridTemplateComponentBase<T>
{
	
}*/