using System;

namespace DragonSpark.Contracts.Messaging;

public readonly record struct IdentifiedMessage(string Message, Guid? Identifier)
{
	public static implicit operator IdentifiedMessage(string message) => new(message);
    
	public static implicit operator IdentifiedMessage(Guid identity) => new(identity.ToString(), identity);

	public IdentifiedMessage(string message) : this(message, Guid.TryParse(message, out var parsed) ? parsed : null) {}
}