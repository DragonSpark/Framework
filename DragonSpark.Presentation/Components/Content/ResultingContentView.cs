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
	RenderFragment? _fragment;
	readonly Switch _loaded = new();
	Func<Task>      _update = default!;
	Worker<T?>?     _subject;

	[Parameter]
	public IResulting<T?>? Content
	{
		get => _content;
		set
		{
			if (_content != value)
			{
				_content = value;
				_loaded.Down();
				_subject = null;
			}
		}
	}

	IResulting<T?>? _content;

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
		var first   = _subject is null;
		_subject ??= Working();
		var task = _subject.Value.AsTask();
		return task.IsCompletedSuccessfully
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
			var result  = _subject.Value().Status.Result;
			var refresh = _fragment is not null;
			_fragment = result is not null ? ChildContent(result) : NotFoundTemplate;
			if (result is not null)
			{
				var callback = refresh ? Refreshed : Rendered;
				if (callback.HasDelegate)
				{
					return callback.InvokeAsync(result);
				}
			}
		}

		return Task.CompletedTask;
	}
}