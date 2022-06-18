namespace DragonSpark.Presentation.Components.Content.Rendering.Sequences;

sealed class AnyKey : RenderContentSubKey
{
	public AnyKey(IRenderContentKey source) : base(source, "any") {}
}