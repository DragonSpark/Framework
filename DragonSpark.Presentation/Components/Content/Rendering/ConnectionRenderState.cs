using DragonSpark.Application.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ConnectionRenderState : MemoryVariable<RenderState?>
{
	public ConnectionRenderState(IMemoryCache memory, CurrentRenderKey key)
		: base(memory, key.Get, PreRenderExpiration.Default) {}
}