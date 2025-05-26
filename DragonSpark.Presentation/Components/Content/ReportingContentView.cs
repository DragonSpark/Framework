using DragonSpark.Application.Runtime.Operations;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

partial class ReportingContentView<TIn, TOut> where TIn : class
{
	Action       _call   = null!;
	Action<Task> _report = null!;

	protected override ValueTask Initialize()
	{
		_call   = Update;
		_report = Report;
		return base.Initialize();
	}

	[Parameter]
	public TIn? Content
	{
		get;
		set
		{
			if (field != value)
			{
				field                  = value;
				Current                = null;
				Subject                = default;
				Loaded                 = true;
				CurrentLoadingTemplate = null;
			}
		}
	}

	[Parameter]
	public IReporter<TIn, TOut> Reporter { get; set; } = null!;

	bool Loaded { get; set; } = true;

	RenderFragment? CurrentLoadingTemplate { get; set; }

	TOut? Subject { get; set; }

	Worker? Current { get; set; }

	protected override void OnParametersSet()
	{
		Subject ??= Content is not null ? Reporter.Get(new(Content, _report)) : default;
	}

	protected override Task OnParametersSetAsync() => Current?.AsTask() ?? base.OnParametersSetAsync();

	void Report(Task parameter)
	{
		CurrentLoadingTemplate = Current == null ? LoadingTemplate : null;
		Current                = Workers.Default.Get(new (parameter, _call));
		Update();
	}

	void Update()
	{
		Loaded = Current is { Status.IsCompletedSuccessfully: true };
		StateHasChanged();
	}
}