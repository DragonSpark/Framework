namespace DragonSpark.Presentation.Components.State;

public sealed class UpdateStateMessage
{
	public UpdateStateMessage(object? subject) => Subject = subject;

	public object? Subject { get; }
}