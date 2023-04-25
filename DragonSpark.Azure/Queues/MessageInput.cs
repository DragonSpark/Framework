using System;

namespace DragonSpark.Azure.Queues;

public readonly record struct MessageInput(string Message, TimeSpan? Visibility, TimeSpan? Life);