using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Content.Rendering.Sequences;

sealed class RenderStateAwareAnyContents<T> : ISelect<RenderStateAwareAnyInput<T>, IAny<T>>
{
	readonly RenderCache        _memory;
	readonly CurrentRenderState _state;
	readonly AnyKey             _key;

	public RenderStateAwareAnyContents(RenderCache memory, CurrentRenderState state, AnyKey key)
	{
		_memory = memory;
		_state  = state;
		_key    = key;
	}

	public IAny<T> Get(RenderStateAwareAnyInput<T> parameter)
	{
		var (owner, source) = parameter;
		var key = _key.Get(owner);
		return new Selection(source, _state, new(_memory, key));
	}

	sealed class Selection : RenderAwareSelection<AnyInput<T>, bool>, IAny<T>
	{
		public Selection(IAny<T> previous, CurrentRenderState state, RenderVariable<bool> variable)
			: base(previous, state, variable) {}
	}
}