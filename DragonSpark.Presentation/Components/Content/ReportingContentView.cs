using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.Content;

partial class ReportingContentView<TIn, TOut> where TIn : class
{
	bool              _ready;
	Action<Task>      _start  = null!;
	RenderFragment?   _view;
	TOut?             _instance;
	Task?             _worker;
	IAlteration<Task> _workers = null!;
	Exception?        _exception;

	protected override void OnInitialized()
	{
		_workers = new Workers(Update);
		_start   = Start;
		base.OnInitialized();
	}

	[Parameter]
	public TIn? Content { get; set; }

	[Parameter, EditorRequired]
	public required IReporter<TIn, TOut> Reporter { get; set; }

	public override Task SetParametersAsync(ParameterView parameters)
	{
		if (parameters.DidParameterChange(nameof(Content), Content))
		{
			_worker   = null;
			_instance = default;
			_ready    = false;
			_view     = null;
		}
		return base.SetParametersAsync(parameters);
	}

	protected override void OnParametersSet()
	{
		_instance ??= Content is not null ? Reporter.Get(new(Content, _start)) : default;
	}

	protected override Task OnParametersSetAsync() => _worker ?? base.OnParametersSetAsync();

	void Start(Task parameter)
	{
		_view   = LoadingTemplate;
		_worker = _workers.Get(parameter);
		Update(_worker);
	}

	void Update(Task parameter)
	{
		_exception = parameter.Exception;
		_ready     = parameter is { IsCompletedSuccessfully: true };
		try
		{
			StateHasChanged();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
}