using System;

namespace DragonSpark.Contracts.Messaging;

public readonly record struct MessageProperties(
	MessageBody Body,
	TimeSpan? Visibility = null,
	TimeSpan? Life = null);