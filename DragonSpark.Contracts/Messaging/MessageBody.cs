namespace DragonSpark.Contracts.Messaging;

public readonly record struct MessageBody(string Message, Guid? Identifier)
{
	public static implicit operator MessageBody(string message) => new(message);
	
	public static implicit operator MessageBody(Guid identity) => new(identity.ToString(), identity);

	public MessageBody(string message) : this(message, Guid.TryParse(message, out var parsed) ? parsed : null) {}

	public MessageBody(Guid message) : this(message.ToString(), message) {}
}