namespace DragonSpark.Presentation.Components.State;

public readonly record struct ActivityReceiverState(object Instance, ActivityOptions Options)
{
	public ActivityReceiverState(object Instance) : this(Instance, ActivityOptions.Default) {}
}