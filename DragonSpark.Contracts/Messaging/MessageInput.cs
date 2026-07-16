using System;

namespace DragonSpark.Contracts.Messaging;

public readonly record struct MessageInput(string Message, TimeSpan? Visibility = null, TimeSpan? Life = null)
{
	public static implicit operator MessageInput(string message) => new(message);
}