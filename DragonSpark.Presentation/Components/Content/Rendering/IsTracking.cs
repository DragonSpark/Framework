using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class IsTracking : ICondition
{
	readonly CurrentRenderState _current;

	public IsTracking(CurrentRenderState current) => _current = current;

	public bool Get(None parameter) => _current.Get() is RenderState.Default or RenderState.Ready;
}