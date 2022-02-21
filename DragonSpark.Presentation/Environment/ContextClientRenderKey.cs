using DragonSpark.Application.Components;
using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

public sealed class ContextClientRenderKey : ClientRenderKey
{
	public ContextClientRenderKey(IClientIdentifier identifier) : base(identifier, A.Type<HttpContext>()) {}
}