namespace DragonSpark.Presentation.Components.State;

public readonly record struct ActivityReaderInstance(object Instance, ActivityReceiverInput Input)
{
	public ActivityReaderInstance(object Instance) : this(Instance, ActivityReceiverInput.Default) {}
}