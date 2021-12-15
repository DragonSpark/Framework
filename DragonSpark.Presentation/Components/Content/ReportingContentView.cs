using DragonSpark.Application.Runtime.Operations;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

partial class ReportingContentView<TIn, TOut> where TIn : class
{
	Action       _call   = default!;
	Action<Task> _report = default!;

	protected override ValueTask Initialize()
	{
		_call   = Update;
		_report = Report;
		return base.Initialize();
	}

	[Parameter]
	public TIn? Content
	{
		get => _content;
		set
		{
			if (_content != value)
			{
				_content = value;
				Loaded   = false;
				Current  = null;
				Subject  = default;
			}
		}
	}	TIn? _content;

	[Parameter]
	public IReporter<TIn, TOut> Reporter { get; set; } = default!;

	TOut? Subject { get; set; }

	Worker? Current { get; set; }

	bool Loaded { get; set; }

	protected override void OnParametersSet()
	{
		Subject ??= Content != null ? Reporter.Get(new(Content, _report)) : default;
	}

	protected override Task OnParametersSetAsync() => Current?.AsTask() ?? base.OnParametersSetAsync();

	void Report(Task parameter)
	{
		Current = Workers.Default.Get(new (parameter, _call));
		Update();
	}

	void Update()
	{
		Loaded = Current is { Status.IsCompletedSuccessfully: true };
		StateHasChanged();
	}
}