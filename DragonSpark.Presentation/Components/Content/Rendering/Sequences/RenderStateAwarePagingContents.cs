using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content.Rendering.Sequences;

sealed class RenderStateAwarePagingContents<T> : ISelect<RenderStateAwarePagingContentsInput<T>, IPages<T>>
{
	readonly RenderCache       _memory;
	readonly IRenderState      _state;
	readonly IRenderContentKey _key;

	public RenderStateAwarePagingContents(RenderCache memory, IRenderState state, IRenderContentKey key)
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

	sealed class Selection : RenderAwareSelection<Stop<PageInput>, PageResult<T>>, IPages<T>
	{
		public Selection(IPages<T> previous, IRenderState state, RenderVariable<PageResult<T>> variable)
			: base(previous, state, variable) {}
	}
}