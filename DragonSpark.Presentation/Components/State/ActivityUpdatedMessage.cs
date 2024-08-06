namespace DragonSpark.Presentation.Components.State;

public sealed record ActivityUpdatedMessage(IActivityMonitor Owner, bool Active)
{
	public ActivityUpdatedMessage(IActivityMonitor Owner) : this(Owner, Owner.Active) {}
}