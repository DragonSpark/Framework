using System;

namespace DragonSpark.Contracts.Messaging;

public readonly record struct MessageInput(string Message, TimeSpan? Visibility = null, TimeSpan? Life = null);

// TODO

public readonly record struct MessageProperties(
	IdentifiedMessage Message,
	TimeSpan? Visibility = null,
	TimeSpan? Life = null);

public readonly record struct DistributedMessageProperties(
	Guid? Identifier,
	string Message,
	string Destination,
	TimeSpan? Visibility = null,
	TimeSpan? Life = null
)
{
	// ReSharper disable once TooManyDependencies
	public DistributedMessageProperties(Guid Identifier, string Destination, TimeSpan? Visibility = null,
	                                    TimeSpan? Life = null)
		: this(Identifier, Identifier.ToString(), Destination, Visibility, Life) {}
}