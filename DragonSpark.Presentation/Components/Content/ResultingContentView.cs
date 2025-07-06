using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

partial class ResultingContentView<T>
{
	readonly Switch _loaded = false;
	RenderFragment? _fragment;
	Func<Task>      _update = null!;
	Worker<T?>?     _subject;

	[Parameter]
	public IResulting<T?>? Content
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				_loaded.Down();
				_subject = null;
			}
		}
	}

	[Parameter]
	public ICondition<None>? UpdateMonitor { get; set; }

	[Parameter]
	public EventCallback<T> Rendered { get; set; }

	[Parameter]
	public EventCallback<T> Refreshed { get; set; }

	[Parameter]
	public bool ForceRender { get; set; }

	protected override void OnInitialized()
	{
		_update = Update;
		base.OnInitialized();
	}

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		Load();
	}

	void Load()
	{
		if (UpdateMonitor?.Get() ?? false)
		{
			_loaded.Down();
			_subject = null;
		}
	}

	Worker<T?> Working() => new WorkingResult<T?>(Content ?? Defaulting<T>.Default, _update).Get();

	protected override Task OnParametersSetAsync()
	{
		var first = _subject is null;
		_subject ??= Working();
		var task = _subject.Value.AsTask();
		var successfully = task.IsCompletedSuccessfully;
		return successfully
				   ? Update()
				   : first && (ForceRender || Render.Get() > RenderState.Default) && !(_fragment is null ? Rendered : Refreshed).HasDelegate
					   ? task
					   : base.OnParametersSetAsync();
	}

	Task Update()
	{
		if (_subject is { Status.IsCompletedSuccessfully: true } && _loaded.Up())
		{
			// ReSharper disable once AsyncApostle.AsyncWait
			var result = _subject.Value().Status.Result;
			var refresh = _fragment is not null;
			_fragment = ContentTemplate?.Invoke(result) ??
			            (result is not null ? ChildContent(result) : NotFoundTemplate);
			if (result is not null)
			{
				var callback = refresh ? Refreshed : Rendered;
				return callback.Invoke(result);
			}
		}

		return Task.CompletedTask;
	}
}