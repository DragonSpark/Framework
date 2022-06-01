using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateAwareActiveContents<T> : ISelect<RenderStateContentInput<T>, IActiveContent<T>>
{
	readonly IMemoryCache       _memory;
	readonly CurrentRenderState _state;

	public RenderStateAwareActiveContents(IMemoryCache memory, CurrentRenderState state)
	{
		_memory = memory;
		_state = state;
	}

	public IActiveContent<T> Get(RenderStateContentInput<T> parameter)
	{
		var (previous, key) = parameter;
		var content = new RenderStateContent<T>(previous, new(_memory, key));
		return new RenderStateAwareActiveContent<T>(previous.Monitor, _state, content);
	}
}