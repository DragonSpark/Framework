namespace DragonSpark.Presentation.Components.State;

public readonly record struct ActivityReceiver(object Instance, ActivityReceiverInput Input)
{
	public ActivityReceiver(object Instance) : this(Instance, ActivityReceiverInput.Default) {}
}