using DragonSpark.Application.Model.Interaction;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Interaction;

public abstract class InjectedInteractionHandlerComponentBase<THandler, T> : ComponentBase
	where THandler : class, IOperation<T>
	where T : IInteractionResult
{
	[Parameter]
	public IInteractionResult Result
	{
		get => _result;
		set
		{
			if (_result != value)
			{
				_result = value;
				Current = null;
			}
		}
	}	IInteractionResult _result = NoActionResult.Default;

	[Inject]
	THandler Operation { get; set; } = default!;

	IInteractionResultHandler Subject { get; set; } = default!;

	Task? Current { get; set; }

	protected override void OnInitialized()
	{
		Subject = Operation.Then().Adapt();
		base.OnInitialized();
	}

	protected override Task OnParametersSetAsync()
	{
		Current ??= Subject.Get(Result).AsTask();
		return Current;
	}
}