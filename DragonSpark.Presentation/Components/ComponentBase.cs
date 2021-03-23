﻿using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
	{
		readonly Action _state;

		public ComponentBase() => _state = StateHasChanged;

		protected override Task OnInitializedAsync() => Execute.Get(GetType(), Initialize()).AsTask();

		protected virtual ValueTask RefreshState() => InvokeAsync(_state).ToOperation();

		protected virtual ValueTask Initialize() => Task.CompletedTask.ToOperation();

		[Inject]
		protected IExceptions Exceptions { get; [UsedImplicitly]set; } = default!;

		// TODO: Remove:
		[Inject]
		protected IExecuteOperation Execute { get; [UsedImplicitly]set; } = default!;
	}
}