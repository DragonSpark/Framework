using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ShouldClearKeys : ICondition
{
	readonly CurrentRenderState _state;

	public ShouldClearKeys(CurrentRenderState state) => _state = state;

	public bool Get(None parameter) => _state.Get() == RenderState.Ready;
}