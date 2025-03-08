using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class ValidationStateMonitor : ComponentBase, IDisposable
{
	readonly Switch _start = new();

	Func<Task>       _update = null!;
	IOperation<None> _call   = null!;

	protected override void OnInitialized()
	{
		_call = new ThrottleOperation<None>(StartUpdate, TimeSpan.FromMilliseconds(100)).Then().Out();
		_update = Update;
		base.OnInitialized();
	}

	Task StartUpdate(None _) => InvokeAsync(_update);

	Task Update() => Changed.Invoke();

	[Parameter]
	public EventCallback Changed { get; set; }

	[CascadingParameter]
	EditContext? EditContext
	{
		get => _subject;
		set
		{
			if (_subject != value)
			{
				if (_subject is not null)
				{
					_subject.OnValidationStateChanged -= StateChanged;
				}

				if ((_subject = value) != null)
				{
					_subject.OnValidationStateChanged += StateChanged;
				}
			}
		}
	}	EditContext? _subject;

	void StateChanged(object? sender, ValidationStateChangedEventArgs e)
	{
		if (_start.Up())
		{
			StateHasChanged();
		}
	}

	protected override Task OnAfterRenderAsync(bool firstRender)
		=> _start.Down() ? _call.Allocate() : base.OnAfterRenderAsync(firstRender);

	public void Dispose()
	{
		EditContext = null;
	}
}