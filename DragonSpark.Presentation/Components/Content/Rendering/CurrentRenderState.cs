using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class CurrentRenderState : Result<RenderState>, IRenderState
{
	public CurrentRenderState(RenderStateStore result) : base(result) {}
}