using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Connections.Events;

public readonly record struct SubscriberInput(uint? Recipient, Func<CancellationToken, Task> Body);

public readonly record struct SubscriberInput<T>(uint? Recipient, Func<Stop<T>, Task> Body);