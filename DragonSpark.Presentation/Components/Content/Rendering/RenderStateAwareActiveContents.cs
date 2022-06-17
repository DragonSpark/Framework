using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateAwareActiveContents<T> : ISelect<ActiveContentInput<T>, IResulting<T?>>
{
	readonly IMemoryCache       _memory;
	readonly SessionRenderState _state;
	readonly IRenderContentKey  _key;

	public RenderStateAwareActiveContents(IMemoryCache memory, SessionRenderState state, IRenderContentKey key)
	{
		_memory = memory;
		_state  = state;
		_key    = key;
	}

	public IResulting<T?> Get(ActiveContentInput<T> parameter)
	{
		var (owner, source) = parameter;
		var key = _key.Get(owner);
		return new RenderAwareResult<T?>(_state, new RenderVariable<T?>(_memory, key), source);
	}
}