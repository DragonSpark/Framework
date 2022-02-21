using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Connections.Initialization;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class ContextHostRenderKey : HostRenderKey<HttpContext>
{
	public static ContextHostRenderKey Default { get; } = new();

	ContextHostRenderKey()
		: base(ClientIdentifierAccessor.Default.Then().Select(x => x.Value().ToString()).Get(),
		       A.Type<HttpContext>()) {}
}