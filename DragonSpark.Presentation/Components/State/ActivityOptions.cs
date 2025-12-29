namespace DragonSpark.Presentation.Components.State;

public record ActivityOptions(bool RedrawOnStart = true, bool RedrawOnFinish = true)
{
	public static ActivityOptions Default { get; } = new();
	
	public static ActivityOptions SkipPostRedraw { get; } = new(RedrawOnFinish: false);
}