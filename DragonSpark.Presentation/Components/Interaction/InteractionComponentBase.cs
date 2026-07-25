using DragonSpark.Application.Model.Interaction;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Interaction;

public class InteractionComponentBase : ComponentBase
{
	[Parameter]
	public IInteractionResultHandler Handler { get; set; } = null!;

	[CascadingParameter]
	IInteractionResult Result { get; set; } = null!;

	protected override ValueTask Initialize() => Handler.Get(Result);
}