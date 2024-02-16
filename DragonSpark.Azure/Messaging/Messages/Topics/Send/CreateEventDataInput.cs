namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public readonly record struct CreateEventDataInput(uint? Recipient, object Message)
{
	public CreateEventDataInput(object Message) : this(null, Message) {}
}