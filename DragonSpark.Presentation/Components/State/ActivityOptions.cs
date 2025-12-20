namespace DragonSpark.Presentation.Components.State;

public record ActivityOptions(bool RedrawOnStart = false, bool RedrawOnFinish = false)
{
	public static ActivityOptions Default { get; } = new();
	public static ActivityOptions Redraw { get; } = new(true, true); // TODO: Audit
	public static ActivityOptions PostRedraw { get; } = new(RedrawOnFinish: true);
}