using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components;

public class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
{
	readonly Action _changed;

	public ComponentBase() => _changed = StateHasChanged;

	protected override Task OnInitializedAsync() => ExecuteOperation.Get(GetType(), Initialize()).AsTask();

	protected virtual ValueTask Initialize() => ValueTask.CompletedTask;

	protected virtual ValueTask RefreshState() => DefaultRefreshState();

	protected ValueTask DefaultRefreshState() => InvokeAsync(_changed).ToOperation();

	[Inject]
	protected IExceptions Exceptions { get; set; } = null!;

	[Inject]
	protected IExecuteOperation ExecuteOperation { get; set; } = null!;

	[CascadingParameter]
	public required CancellationToken Stop { get; set; } = CancellationToken.None;
}