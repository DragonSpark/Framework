using DragonSpark.Presentation.Interaction;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Interaction;

public class InteractionComponentBase : ComponentBase
{
	[Parameter]
	public IInteractionResultHandler Handler { get; set; } = default!;

	[CascadingParameter]
	IInteractionResult Result { get; set; } = default!;

	protected override ValueTask Initialize() => Handler.Get(Result);
}