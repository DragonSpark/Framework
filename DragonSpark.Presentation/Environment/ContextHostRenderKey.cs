using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class ContextHostRenderKey : HostRenderKey<HttpContext>
{
	public ContextHostRenderKey(ClientSessionIdentifier identifier)
		: base(identifier, A.Type<ContextHostRenderKey>()) {}
}