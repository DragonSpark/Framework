using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Interaction;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Interaction
{
	public abstract class InjectedInteractionHandlerComponentBase<THandler, T> : ComponentBase
		where THandler : class, IOperation<T>
		where T : IInteractionResult
	{
		[Parameter]
		public IInteractionResult Result { get; set; } = default!;

		[Inject]
		THandler Operation { get; set; } = default!;

		protected override ValueTask Initialize() => Operation.Then().Adapt().Get(Result);
	}
}