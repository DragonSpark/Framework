using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Environment;

public sealed class ContextRenderApply : RenderApply<HttpContext>
{
	public ContextRenderApply(IMemoryCache memory, ContextClientRenderKey key, ContextStore store)
		: base(memory, key, store) {}
}