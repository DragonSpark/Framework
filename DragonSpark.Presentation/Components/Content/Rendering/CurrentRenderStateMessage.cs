namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class CurrentRenderStateMessage : DragonSpark.Model.Results.Instance<RenderState>
{
	public CurrentRenderStateMessage(RenderState state) : base(state) {}
}