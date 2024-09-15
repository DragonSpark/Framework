using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components;

public class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
{
	readonly Action _changed;

	public ComponentBase() => _changed = StateHasChanged;

	protected override Task OnInitializedAsync() => Execute.Get(GetType(), Initialize()).AsTask();

	protected virtual ValueTask Initialize() => ValueTask.CompletedTask;

	protected virtual ValueTask RefreshState() => DefaultRefreshState();

	ValueTask DefaultRefreshState() => InvokeAsync(_changed).ToOperation();

	[Inject]
	protected IExceptions Exceptions { get; set; } = default!;

	[Inject]
	protected IExecuteOperation Execute { get; set; } = default!;
}