using DragonSpark.Application.Diagnostics;
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

		protected virtual ValueTask Initialize() => Task.CompletedTask.ToOperation();

		protected virtual ValueTask RefreshState() => InvokeAsync(_state).ToOperation();

		[Inject]
		protected IExceptions Exceptions { get; [UsedImplicitly]set; } = default!;

		[Inject]
		protected IExecuteOperation Execute { get; [UsedImplicitly]set; } = default!;
	}
}