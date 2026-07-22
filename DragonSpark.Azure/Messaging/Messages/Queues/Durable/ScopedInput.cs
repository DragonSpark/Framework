using System;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

public readonly record struct ScopedInput(TimeSpan? Visibility = null, TimeSpan? Life = null);