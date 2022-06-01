using DragonSpark.Application.Components;
using DragonSpark.Application.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ConnectionRenderState : MemoryVariable<RenderState?>
{
	public ConnectionRenderState(IMemoryCache memory, IClientIdentifier identifier)
		: base(memory, ConnectionRenderKey.Default.Get(identifier.Get()), PreRenderExpiration.Default) {}
}