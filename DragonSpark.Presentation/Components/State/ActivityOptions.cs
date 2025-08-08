namespace DragonSpark.Presentation.Components.State;

public record ActivityOptions(bool RedrawOnStart = false, PostRenderAction PostRenderAction = PostRenderAction.None)
{
	public static ActivityOptions Default { get; } = new();
	public static ActivityOptions PostRedraw { get; } = new(PostRenderAction: PostRenderAction.ForceRedraw);
	public static ActivityOptions SkipPostRender { get; } = new(PostRenderAction: PostRenderAction.DeferredRedraw);
}