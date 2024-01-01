namespace DragonSpark.Azure.Events;

sealed class EventType : Text.Text
{
	public static EventType Default { get; } = new();

	EventType() : base(nameof(EventType)) {}
}

// TODO

sealed class Recipient : Text.Text
{
	public static Recipient Default { get; } = new();

	Recipient() : base(nameof(Recipient)) {}
}