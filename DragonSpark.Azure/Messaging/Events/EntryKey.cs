namespace DragonSpark.Azure.Messaging.Events;

public sealed record EntryKey(uint? Recipient, string MessageType)
{
	public EntryKey(string MessageType) : this(null, MessageType) {}
}