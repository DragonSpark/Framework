using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ComponentRenderState : TableVariable<string, RenderState>
{
	public ComponentRenderState(string key, RenderStates store) : base(key, store) {}
}