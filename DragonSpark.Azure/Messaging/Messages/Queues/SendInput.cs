using System;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

public readonly record struct SendInput(TimeSpan? Life = null, TimeSpan? Visibility = null);