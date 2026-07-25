using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class IsLoading : ICondition
{
	readonly RenderStateStore _state;

	public IsLoading(RenderStateStore state) => _state = state;

	public bool Get(None parameter) => _state.Get() < RenderState.Established;
}