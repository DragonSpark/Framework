using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateAwareActiveContents<T> : ISelect<RenderStateContentInput<T>, IActiveContent<T>>
{
	readonly IMemoryCache _memory;
	readonly RenderStates _states;

	public RenderStateAwareActiveContents(IMemoryCache memory, RenderStates states)
	{
		_memory = memory;
		_states = states;
	}

	public IActiveContent<T> Get(RenderStateContentInput<T> parameter)
	{
		var (previous, key) = parameter;
		var content = new RenderStateContent<T>(previous, _memory, key);
		var state   = new ComponentRenderState(key, _states);
		return new RenderStateAwareActiveContent<T>(previous, state, content);
	}
}