namespace DragonSpark.Presentation.Components.State;

public sealed record ActivityUpdatedMessage(IActivityMonitor Owner, bool Active, bool Force)
{
	public ActivityUpdatedMessage(IActivityMonitor Owner, bool Force = false) : this(Owner, Owner.Active, Force) {}
}