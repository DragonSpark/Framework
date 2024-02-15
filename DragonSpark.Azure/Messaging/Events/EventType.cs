namespace DragonSpark.Azure.Messaging.Events;

sealed class EventType : Text.Text
{
	public static EventType Default { get; } = new();

	EventType() : base(nameof(EventType)) {}
}