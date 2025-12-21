namespace DragonSpark.Presentation.Components.State;

// TODO: If target is the element, RedrawOnFinish=true.  If target is the ActivityReceiver, RedrawOnFinish=false
public record ActivityOptions(bool RedrawOnStart = true, bool RedrawOnFinish = true)
{
	public static ActivityOptions Default { get; } = new();
	public static ActivityOptions Redraw { get; } = new(); // TODO: Audit

	
}