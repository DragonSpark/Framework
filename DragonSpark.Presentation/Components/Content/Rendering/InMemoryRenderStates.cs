using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class InMemoryRenderStates : Select<Guid, RenderStates>
{
	public InMemoryRenderStates(IMemoryCache memory) : this(memory, PreRenderingWindow.Default) {}

	public InMemoryRenderStates(IMemoryCache memory, TimeSpan window)
		: base(Start.A.Selection<Guid>()
		            .By.Instantiation<RenderStates>()
		            .Store()
		            .In(memory)
		            .For(window.Slide())
		            .Using(RenderStatesKey.Default)) {}
}