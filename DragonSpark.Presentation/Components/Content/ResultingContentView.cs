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
	}	IResulting<T?>? _content;


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
		_subject ??= new WorkingResult<T?>(Content ?? Defaulting<T>.Default, _update).Get();
	}

	protected override Task OnParametersSetAsync()
	{
		if (_subject is not null)
		{
			var task = _subject.Value.AsTask();
			return task.IsCompletedSuccessfully ? Update(false) : ForceRender || Render.Get() > RenderState.Default ? task : base.OnParametersSetAsync();
		}

		return base.OnParametersSetAsync();
	}

	Task Update() => Update(true);

	Task Update(bool update)
	{
		if (UpdateMonitor?.Get() ?? false)
		{
			_loaded.Down();
			_subject = null;
			Load();
			return Task.CompletedTask;
		}

		return Complete(update);
	}

	Task Complete(bool update)
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

			if (update)
			{
				StateHasChanged();
			}
		}

		return Task.CompletedTask;
	}
}