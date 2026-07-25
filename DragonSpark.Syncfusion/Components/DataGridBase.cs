using BlazorPro.BlazorSize;
using DragonSpark.Compose;
using DragonSpark.Presentation.Components;
using DragonSpark.Presentation.Components.Content.Templates;
using DragonSpark.SyncfusionRendering.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;
using FilterType = Syncfusion.Blazor.Grids.FilterType;

namespace DragonSpark.SyncfusionRendering.Components;

public class DataGridBase<T> : DataComponent
{
	protected DataGrid<T>? _subject;
	protected string       _identity   = string.Empty;
	string                 _identifier = string.Empty;

	protected override void OnInitialized()
	{
		base.OnInitialized();
		_identifier = GenerateElementIdentifier.Default.Get(Id);
	}

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Qualifier), Qualifier);
		await base.SetParametersAsync(parameters).Off();
		_identity = Identifier ?? (changed || _identity.IsNullOrEmpty() ? $"{_identifier}{Qualifier}" : _identity);
	}

	[Parameter]
	public Guid Id { get; set; }

	[Parameter]
	public string? Identifier { get; set; }

	[Parameter]
	public string Qualifier { get; set; } = string.Empty;

	[Parameter]
	public FilterType FilterType { get; set; } = FilterType.Excel;

	[Parameter]
	public bool AllowExport { get; set; }

	[Parameter]
	public bool AllowSelection { get; set; }

	[Parameter]
	public RenderFragment Columns { get; set; } = null!;

	[Parameter]
	public string Breakpoint { get; set; } = Breakpoints.SmallUp;

	[Parameter]
	public RenderFragment EmptyElementsTemplate { get; set; } = DefaultEmptyResultTemplate.Default;
	
	[Parameter]
	public required RenderFragment<Exception> ProblemTemplate { get; set; } = DefaultExceptionTemplate.Default;
	[Parameter]
	public ICollection<string>? DesktopToolbar { get; set; }

	[Parameter]
	public ICollection<string> MobileToolbar { get; set; } = DefaultToolbar.Default;

	[Parameter]
	public EventCallback<Updated<T>> Updated { get; set; }

	[Parameter]
	public EventCallback<Allow<T>> Editing { get; set; }

	[Parameter]
	public EventCallback<T> Created { get; set; }

	[Parameter]
	public EventCallback Ready { get; set; }

	[Parameter]
	public EventCallback Empty { get; set; }

	[Parameter]
	public EventCallback<Exception> Error { get; set; }

	public Task Export() => _subject?.Export() ?? Task.CompletedTask;

	public Task Refresh() => _subject?.Refresh() ?? Task.CompletedTask;
}