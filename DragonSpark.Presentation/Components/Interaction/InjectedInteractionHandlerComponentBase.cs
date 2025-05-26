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
		get;
		set
		{
			if (field != value)
			{
				field   = value;
				Current = null;
			}
		}
	} = NoActionResult.Default;

	[Inject]
	THandler Operation { get; set; } = null!;

	IInteractionResultHandler Subject { get; set; } = null!;

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