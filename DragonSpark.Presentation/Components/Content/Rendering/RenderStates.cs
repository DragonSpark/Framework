using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStates : StandardTable<string, RenderState>
{
	public RenderStates() : base(_ => RenderState.Default) {}
}