using DragonSpark.Application.Components;
using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;

namespace DragonSpark.Presentation.Environment;

public sealed class ContextClientRenderKey : ClientRenderKey
{
	public ContextClientRenderKey(IClientIdentifier identifier) : base(identifier, A.Type<ContextClientRenderKey>()) {}
}