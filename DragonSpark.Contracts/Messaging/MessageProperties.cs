using System;

namespace DragonSpark.Contracts.Messaging;

public readonly record struct MessageProperties(
	IdentifiedMessage Message,
	TimeSpan? Visibility = null,
	TimeSpan? Life = null);