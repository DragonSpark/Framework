using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content.Rendering.Sequences;

sealed class RenderStateAwarePagingContents<T> : ISelect<RenderStateAwarePagingContentsInput<T>, IPages<T>>
{
	readonly RenderCache        _memory;
	readonly RenderStateStore _state;
	readonly IRenderContentKey  _key;

	public RenderStateAwarePagingContents(RenderCache memory, RenderStateStore state, IRenderContentKey key)
	{
		_memory = memory;
		_state  = state;
		_key    = key;
	}

	public IPages<T> Get(RenderStateAwarePagingContentsInput<T> parameter)
	{
		var (owner, source) = parameter;
		var key = _key.Get(owner);
		return new Selection(source, _state, new(_memory, key));
	}

	sealed class Selection : RenderAwareSelection<PageInput, Page<T>>, IPages<T>
	{
		public Selection(IPages<T> previous, RenderStateStore state, RenderVariable<Page<T>> variable)
			: base(previous, state, variable) {}
	}
}