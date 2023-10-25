using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateAwareActiveContents<T> : ISelect<ActiveContentInput<T>, IResulting<T?>>
{
	readonly RenderCache        _memory;
	readonly RenderStateStore _state;
	readonly IRenderContentKey  _key;

	public RenderStateAwareActiveContents(RenderCache memory, RenderStateStore state, IRenderContentKey key)
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