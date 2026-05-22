using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

partial class ResultingContentView<T> : ICompleted<T?>
{
	readonly Switch _render = true;
	RenderFragment? _fragment;
	Worker?         _subject;
	Workers<T?>     _workers = null!;
	Exception?      _exception;

	protected override void OnInitialized()
	{
		_workers = new Workers<T?>(this);
		base.OnInitialized();
	}

	[Parameter, EditorRequired]
	public required IResulting<T?> Content { get; set; }

	protected override bool ShouldRender() => _render || !_render.Up();

	[Parameter]
	public ICondition<None>? UpdateMonitor { get; set; }

	[Parameter]
	public EventCallback<T> Rendering { get; set; }

	[Parameter]
	public EventCallback<T> Rendered { get; set; }

	[Parameter]
	public EventCallback<T> Refreshed { get; set; }

	[Parameter]
	public bool ForceRender { get; set; }

	public override Task SetParametersAsync(ParameterView parameters)
	{
		if (parameters.DidParameterChange(nameof(Content), Content))
		{
			Reset();
		}

		return base.SetParametersAsync(parameters);
	}

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		if (UpdateMonitor?.Get() ?? false)
		{
			_render.Down();
			Reset();
		}
	}

	void Reset()
	{
		_subject?.Dispose();
		_subject = null;
	}

	protected override Task OnParametersSetAsync()
	{
		var @new                            = _subject is null;
		var (monitor, complete) = _subject ??= _workers.Get(Content);
		return monitor.IsCompleted
			       ? complete.Get()
			       : @new && (ForceRender || Render > RenderState.Default) &&
			         !(_fragment is null ? Rendered : Refreshed).HasDelegate
				       ? monitor
				       : base.OnParametersSetAsync();
	}

	public async Task Get(ValueTask<T?> parameter)
	{
		_exception = null;
		if (parameter is { IsCompletedSuccessfully: true })
		{
			// ReSharper disable once AsyncApostle.AsyncWait
			var result = parameter.Result;
			if (result is not null)
			{
				await Rendering.On(result);
			}

			var refresh = _fragment is not null;
			_fragment = ContentTemplate?.Invoke(result) ??
			            (result is not null ? ChildContent(result) : NotFoundTemplate);
			if (result is not null)
			{
				var callback = refresh ? Refreshed : Rendered;
				await callback.InvokeAsync(result).Off();
			}
		}
		else if (parameter is { IsFaulted: true })
		{
			_exception = parameter.AsTask().Exception;
			StateHasChanged();
		}
	}

	public bool Get(IResulting<T?> parameter) => parameter == Content;
}