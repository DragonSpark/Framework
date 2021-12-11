﻿using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components;

public class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
{
	protected override Task OnInitializedAsync() => Execute.Get(GetType(), Initialize()).AsTask();

	protected virtual ValueTask Initialize() => Task.CompletedTask.ToOperation();

	protected virtual ValueTask RefreshState() => DefaultRefreshState();

	protected void CallDefaultRefreshState()
	{
		DefaultRefreshState();
	}

	protected ValueTask DefaultRefreshState() => InvokeAsync(StateHasChanged).ToOperation();
	
	[Inject]
	protected IExceptions Exceptions { get; [UsedImplicitly]set; } = default!;

	[Inject]
	protected IExecuteOperation Execute { get; [UsedImplicitly]set; } = default!;
}