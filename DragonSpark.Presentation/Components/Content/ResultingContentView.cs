using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

partial class ResultingContentView<T>
{
	Func<Task> _update = default!;

	[Parameter]
	public IResulting<T?>? Content
	{
		get => _content;
		set
		{
			if (_content != value)
			{
				_content = value;
				Loaded   = false;
				Subject  = null;
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

	RenderFragment? Fragment { get; set; }

	bool Loaded { get; set; }

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
		Subject ??= new WorkingResult<T?>(Content ?? Defaulting<T>.Default, _update).Get();
	}

	protected override Task OnParametersSetAsync()
	{
		if (Subject != null)
		{
			var task = Subject.Value.AsTask();
			return task.IsCompletedSuccessfully ? Update() : ForceRender || Render.Get() > RenderState.Default ? task : base.OnParametersSetAsync();
		}

		return base.OnParametersSetAsync();
	}

	Worker<T?>? Subject { get; set; }

	protected override Task OnAfterRenderAsync(bool firstRender) => Update();

	Task Update()
	{
		if (UpdateMonitor?.Get() ?? false)
		{
			Loaded  = false;
			Subject = null;
			Load();
		}
		else
		{
			var loaded = Subject is { Status.IsCompletedSuccessfully: true };
			var update = loaded && loaded != Loaded;
			Loaded = loaded;
			if (update)
			{
				// ReSharper disable once AsyncApostle.AsyncWait
				var result  = Subject.Value().Status.Result;
				var refresh = Fragment is not null;
				Fragment = result is not null ? ChildContent(result) : NotFoundTemplate;
				StateHasChanged();
				if (result is not null)
				{
					var callback = refresh ? Refreshed : Rendered;
					return callback.InvokeAsync(result);
				}
			}
		}
		return Task.CompletedTask;
	}
}