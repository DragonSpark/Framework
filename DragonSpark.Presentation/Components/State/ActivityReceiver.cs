namespace DragonSpark.Presentation.Components.State;

public readonly record struct ActivityReceiver(object Instance, ActivityOptions Options)
{
	public ActivityReceiver(object Instance) : this(Instance, ActivityOptions.Default) {}
}